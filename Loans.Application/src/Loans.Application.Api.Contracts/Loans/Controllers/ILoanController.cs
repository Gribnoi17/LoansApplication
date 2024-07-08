using Loans.Application.Api.Contracts.Loans.Requests;
using Loans.Application.Api.Contracts.Loans.Responses;

namespace Loans.Application.Api.Contracts.Loans.Controllers
{
    /// <summary>
    /// Предназначен для работы с кредитными договорами.
    /// </summary>
    public interface ILoanController
    {
        /// <summary>
        /// Создание нового кредитного договора.
        /// </summary>
        /// <param name="request">Данные для регистрации кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Информация о созданном договоре.</returns>
        Task<LoanContractResponse> CreateLoanContract(LoanRequest request, CancellationToken token);

        /// <summary>
        /// Получение кредитного договора по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Информация о кредитном договоре.</returns>
        Task<LoanContractResponse> GetLoanContractById(long id, CancellationToken token);

        /// <summary>
        /// Получение статуса кредитного договора по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Статус кредитного договора.</returns>
        Task<string> CheckStatus(long id, CancellationToken token);
    }
}