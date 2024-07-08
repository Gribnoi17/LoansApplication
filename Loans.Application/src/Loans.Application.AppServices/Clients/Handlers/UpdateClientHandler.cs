using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Clients.Handlers
{
    /// <inheritdoc />
    internal class UpdateClientHandler : IUpdateClientHandler
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<UpdateClientHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса SearchClientHandler c репозиторием клиентов и их валидатором.
        /// </summary>
        /// <param name="clientRepository">Репозиторий клиентов</param>
        /// <param name="logger">Logger сообщений.</param>
        public UpdateClientHandler(IClientRepository clientRepository, ILogger<UpdateClientHandler> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }
        
        public async Task Handle(ClientUpdateInternalRequest request, CancellationToken token)
        {
            _logger.LogInformation("Началось обновление данных клиента с Id: {Id}!", request.Id);
            
            _logger.LogTrace("Получение клиента с Id: {Id}!", request.Id);
            var client = await _clientRepository.GetClientById(request.Id, token);
            
            _logger.LogTrace("Обновляем данные клиента с Id: {Id}!", request.Id);
            UpdateClientData(request, client);
            
            await _clientRepository.UpdateClient(client, token);
            _logger.LogInformation("Обновление данных клиента с Id: {Id} закончено!", request.Id);
        }
        
        private void UpdateClientData(ClientUpdateInternalRequest updateRequest, Client client)
        {
            var validationErrors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(updateRequest.FirstName) == false && updateRequest.FirstName != client.FirstName)
            {
                client.FirstName = updateRequest.FirstName;
            }

            if (string.IsNullOrWhiteSpace(updateRequest.LastName) == false && updateRequest.LastName != client.LastName)
            {
                client.LastName = updateRequest.LastName;
            }

            if (string.IsNullOrWhiteSpace(updateRequest.MiddleName) == false && updateRequest.MiddleName != client.MiddleName)
            {
                client.MiddleName = updateRequest.MiddleName;
            }

            if (updateRequest.Salary.HasValue)
            {
                if (updateRequest.Salary >= 0 && updateRequest.Salary != client.Salary)
                {
                    client.Salary = updateRequest.Salary.Value;
                }
                else if (updateRequest.Salary < 0)
                {
                    validationErrors.Add("Зарплата должна быть больше 0!");
                    throw new ValidationException(validationErrors);
                }
            }
        }
    }
}
