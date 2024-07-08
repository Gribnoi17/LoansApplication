using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Contracts.Loans.Handlers
{
    /// <summary>
    /// Обработчик для получения кредитного договора по идентификатору клиента.
    /// </summary>
    public interface IGetLoanContractsByClientIdHandler
    {
        /// <summary>
        /// Обрабатывает запрос на получение кредитного договора по идентификатору клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Соответствующий кредитный договор.</returns>
        Task<List<LoanContract>> Handle(long clientId, CancellationToken token);
    }
}
