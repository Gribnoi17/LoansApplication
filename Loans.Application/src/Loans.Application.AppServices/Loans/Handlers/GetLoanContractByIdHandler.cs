using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Loans.Handlers
{
    /// <inheritdoc />
    internal class GetLoanContractByIdHandler : IGetLoanContractByIdHandler
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ILogger<GetLoanContractByIdHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса GetLoanContractByIdHandler.
        /// </summary>
        /// <param name="loanContractRepository">Репозиторий кредитных заявок.</param>
        /// <param name="logger">Logger сообщений.</param>
        public GetLoanContractByIdHandler(ILoanContractRepository loanContractRepository, ILogger<GetLoanContractByIdHandler> logger)
        {
            _loanContractRepository = loanContractRepository;
            _logger = logger;
        }
        
        public async Task<LoanContract> Handle(long id, CancellationToken token)
        {
            _logger.LogInformation( "Получение кредитной заявки с ее Id: {Id}!", id);
            return await _loanContractRepository.GetLoanContractById(id, token);
        }
    }
}
