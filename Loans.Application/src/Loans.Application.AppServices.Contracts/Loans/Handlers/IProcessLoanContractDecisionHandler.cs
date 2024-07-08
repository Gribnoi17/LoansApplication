using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Contracts.Loans.Handlers
{
    /// <summary>
    /// Интерфейс для обработки запроса на сохранение кредитной заявки с окончательным статусом и обновления данных в базе данных.
    /// </summary>
    public interface IProcessLoanContractDecisionHandler
    {
        /// <summary>
        /// Обрабатывает запрос на сохранение кредитной заявки с окончательным статусом и обновляет кредитную заявку в базе данных.
        /// </summary>
        /// <param name="loanContractEventResult">Заявка на сохранение кредитной заявки.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task Handle(LoanContractEventResult loanContractEventResult, CancellationToken token);
    }
}