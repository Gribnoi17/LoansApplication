using Loans.Application.DataAccess.Data;
using Loans.Application.DataAccess.Infrastructure.MapService;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace Loans.Application.DataAccess.Clients.Repository
{
    /// <inheritdoc />
    internal class ClientRepository : IClientRepository
    {
        private readonly LoansDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр класса ClientRepository.
        /// </summary>
        /// <param name="context">Контекст базы данных, с которым будет взаимодействовать репозиторий клиентов.</param>
        public ClientRepository(LoansDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetClientById(long id, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<Client>(token);
            }

            var clientEntity = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, token);

            if (clientEntity == null)
            {
                throw new InvalidOperationException($"Клиент с таким Id: {id} не найден!");
            }

            var client = Mapper.MapToClient(clientEntity);

            return client;
        }

        public async Task<List<Client>> SearchClients(ClientInternalFilter internalFilter, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<List<Client>>(token);
            }

            var query = _context.Clients
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(internalFilter.FirstName))
            {
                query = query.Where(c => c.FirstName == internalFilter.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(internalFilter.LastName))
            {
                query = query.Where(c => c.LastName == internalFilter.LastName);
            }

            if (!string.IsNullOrWhiteSpace(internalFilter.MiddleName))
            {
                query = query.Where(c => c.MiddleName == internalFilter.MiddleName);
            }

            if (internalFilter.BirthDate.HasValue)
            {
                query = query.Where(c => c.BirthDate == new DateOnly(internalFilter.BirthDate.Value.Year,
                    internalFilter.BirthDate.Value.Month, internalFilter.BirthDate.Value.Day));
            }
            
            var filteredEntities = await query.ToListAsync(token);

            var filteredClients = Mapper.MapToClients(filteredEntities);

            return await Task.FromResult(filteredClients.ToList());
        }

        public async Task<long> AddClient(Client client, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<long>(token);
            }

            var clientEntity = Mapper.MapToClientEntity(client);
            
            try
            {
                var result = await _context.Clients.AddAsync(clientEntity, token);

                await _context.SaveChangesAsync(token);

                return result.Entity.Id;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("При добавлении данных клиента что-то пошло не так!");
            }
        }

        public async Task UpdateClient(Client client, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                await Task.FromCanceled(token);
                return;
            }
            
            try
            {
                var clientEntity = await _context.Clients.FirstOrDefaultAsync(c => c.Id == client.Id, token);

                if (clientEntity == null)
                {
                    throw new ArgumentException($"Клиент с таким Id: {client.Id} не найден!");
                }

                Mapper.MapToUpdateClientEntity(client, clientEntity);

                await _context.SaveChangesAsync(token);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}