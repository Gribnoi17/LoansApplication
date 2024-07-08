using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Contracts.Loans.Handlers
{
    /// <summary>
    /// Обработчик для получения кредитного договора по его идентификатору.
    /// </summary>
    public interface IGetLoanContractByIdHandler
    {
        /// <summary>
        /// Обрабатывает запрос на получение кредитного договора по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Соответствующий кредитный договор.</returns>
        Task<LoanContract> Handle(long id, CancellationToken token);
    }
}
