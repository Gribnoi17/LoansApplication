using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Repository;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Loans.Handlers
{
    /// <inheritdoc />
    internal class ProcessLoanContractDecisionHandler : IProcessLoanContractDecisionHandler
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ILogger<ProcessLoanContractDecisionHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса GetDecisionHandler с указанными сервисами.
        /// </summary>
        /// <param name="loanContractRepository">Репозиторий кредитных договоров.</param>
        /// <param name="logger">Logger сообщений.</param>
        public ProcessLoanContractDecisionHandler(ILoanContractRepository loanContractRepository, ILogger<ProcessLoanContractDecisionHandler> logger)
        {
            _loanContractRepository = loanContractRepository;
            _logger = logger;
        }
        
        public async Task Handle(LoanContractEventResult loanContractEventResult, CancellationToken token)
        {
            var loanContract = new LoanContract();
            
            ApplyDecisionToLoanContract(loanContract, loanContractEventResult, "Какая-та причина отказа");
            
            _logger.LogInformation( "Обновление данных с решением для кредитного договора с Id заявки: {LoanContractId}!", loanContractEventResult.LoanContractId);
            await _loanContractRepository.UpdateLoanContract(loanContract, token);
        }
        
        /// <summary>
        /// Применяет решение к кредитному договору на основе результата сообщения из Кафки.
        /// </summary>
        /// <param name="loanContract">Кредитный договор для обновления.</param>
        /// <param name="result">Результат события для применения.</param>
        /// <param name="rejectionReason">Причина отказа (применяется только в случае отказа).</param>
        internal static void ApplyDecisionToLoanContract(LoanContract loanContract, LoanContractEventResult result, string rejectionReason)
        {
            loanContract.Id = result.LoanContractId;
            loanContract.ClientId = result.ClientId;
            loanContract.Amount = result.Amount;
            loanContract.LoanTermMonth = result.LoanTermMonth;
            loanContract.InterestRate = result.InterestRate;
            loanContract.LoanDate = result.LoanDate;
            loanContract.Status = result.Status;
            
            if (result.Status != LoanStatus.InProgress && result.Status != LoanStatus.Approved)
            {
                loanContract.RejectionReason = rejectionReason;
            }
        }
    }
}