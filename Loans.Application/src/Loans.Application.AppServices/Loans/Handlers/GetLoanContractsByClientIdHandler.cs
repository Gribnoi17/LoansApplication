using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Loans.Handlers
{
    /// <inheritdoc />
    internal class GetLoanContractsByClientIdHandler : IGetLoanContractsByClientIdHandler
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ILogger<GetLoanContractsByClientIdHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса GetLoanContractsByClientIdHandler с указанными сервисами.
        /// </summary>
        /// <param name="loanContractRepository">Репозиторий кредитных договоров.</param>
        /// <param name="logger">Logger сообщений.</param>
        public GetLoanContractsByClientIdHandler(ILoanContractRepository loanContractRepository, ILogger<GetLoanContractsByClientIdHandler> logger)
        {
            _loanContractRepository = loanContractRepository;
            _logger = logger;
        }
        
        public async Task<List<LoanContract>> Handle(long clientId, CancellationToken token)
        {
            _logger.LogInformation( "Получение кредитных договоров клиента c Id: {СlientId}!", clientId);
            var loanContracts = await _loanContractRepository.GetLoanContractsByClientId(clientId, token);
            
            return loanContracts;
        }
    }
}
