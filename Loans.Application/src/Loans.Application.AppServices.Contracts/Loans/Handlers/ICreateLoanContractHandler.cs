using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Contracts.Loans.Handlers
{
    /// <summary>
    /// Обработчик создания кредитных договоров.
    /// </summary>
    public interface ICreateLoanContractHandler
    {
        /// <summary>
        /// Обрабатывает запрос на создание кредитного договора и возвращает созданный договор.
        /// </summary>
        /// <param name="request">Запрос на создание кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Созданный кредитный договор.</returns>
        Task<LoanContract> Handle(LoanContractInternalRequest request, CancellationToken token);
    }
}
