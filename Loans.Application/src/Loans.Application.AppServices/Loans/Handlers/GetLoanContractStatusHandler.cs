using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Loans.Handlers
{
    /// <inheritdoc />
    internal class GetLoanContractStatusHandler : IGetLoanContractStatusHandler
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ILogger<GetLoanContractStatusHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса GetLoanContractStatusHandler с указанными сервисами.
        /// </summary>
        /// <param name="loanContractRepository">Репозиторий кредитных договоров.</param>
        /// <param name="logger">Logger сообщений.</param>
        public GetLoanContractStatusHandler(ILoanContractRepository loanContractRepository, ILogger<GetLoanContractStatusHandler> logger)
        {
            _loanContractRepository = loanContractRepository;
            _logger = logger;
        }

        public async Task<LoanStatus> Handle(long id, CancellationToken token)
        {
            _logger.LogInformation( "Получение статуса кредитной заявки с ее Id: {Id}!", id);
            var loanContract = await _loanContractRepository.GetLoanContractById(id, token);
            
            return loanContract.Status;
        }
    }
}
