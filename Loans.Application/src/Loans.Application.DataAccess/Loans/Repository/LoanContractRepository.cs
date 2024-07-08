using Loans.Application.DataAccess.Data;
using Loans.Application.DataAccess.Infrastructure.MapService;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Repository;
using Microsoft.EntityFrameworkCore;

namespace Loans.Application.DataAccess.Loans.Repository
{
    /// <inheritdoc />
    internal class LoanContractRepository : ILoanContractRepository
    {
        private readonly LoansDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр класса LoanContractRepository.
        /// </summary>
        /// <param name="context">Контекст базы данных, с которым будет взаимодействовать репозиторий кредитов.</param>
        public LoanContractRepository(LoansDbContext context)
        {
            _context = context;
        }

        public async Task<LoanContract> GetLoanContractById(long id, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<LoanContract>(token);
            }

            var loanContractEntity = await _context.LoanContracts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, token);

            if (loanContractEntity == null)
            {
                throw new InvalidOperationException($"Кредитный договор с таким Id: {id} не найден!");
            }

            var loanContract = Mapper.MapToLoanContract(loanContractEntity);

            return loanContract;
        }

        public async Task<List<LoanContract>> GetLoanContractsByClientId(long id, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<List<LoanContract>>(token);
            }

            var loanContractEntities = await _context.LoanContracts
                .AsNoTracking()
                .Where(l => l.ClientId == id)
                .ToListAsync(token);

            if (loanContractEntities.Count == 0)
            {
                throw new InvalidOperationException($"Кредитные договора с таким Id: {id} клиента не найдены!");
            }

            var loanContracts = Mapper.MapToLoanContracts(loanContractEntities);

            return loanContracts;
        }

        public async Task<long> AddLoanContract(LoanContract loanContract, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<long>(token);
            }

            var loanContractEntity = Mapper.MapToLoanContractEntity(loanContract);

            try
            {
                var result = await _context.LoanContracts.AddAsync(loanContractEntity, token);

                await _context.SaveChangesAsync(token);

                return result.Entity.Id;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("При добавлении кредитного договора что-то пошло не так!");
            }
        }

        public async Task UpdateLoanContract(LoanContract loanContract, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                await Task.FromCanceled(token);
                return;
            }

            try
            {
                var loanContractEntity = await _context.LoanContracts.FirstOrDefaultAsync(l => l.Id == loanContract.Id, token);

                if (loanContractEntity == null)
                {
                    throw new ArgumentException($"Кредитный договор с таким Id: {loanContract.Id} не найдена!");
                }
 
                Mapper.MapToUpdateLoanContractEntity(loanContract, loanContractEntity);

                await _context.SaveChangesAsync(token);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}