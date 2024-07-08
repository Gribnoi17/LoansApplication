using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Loans.Repository
{
    /// <summary>
    /// Репозиторий для работы с кредитными договорами.
    /// </summary>
    public interface ILoanContractRepository
    {
        /// <summary>
        /// Получает кредитный договор по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Кредитный договор с указанным идентификатором.</returns>
        Task<LoanContract> GetLoanContractById(long id, CancellationToken token);
        
        /// <summary>
        /// Получает кредитные договора по идентификатору клиента.
        /// </summary>
        /// <param name="id">Идентификатор клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Кредитный договор с указанным идентификатором клиента.</returns>
        Task<List<LoanContract>> GetLoanContractsByClientId(long id, CancellationToken token);
        
        /// <summary>
        /// Добавляет кредитный договор в память.
        /// </summary>
        /// <param name="loanContractInternalRequest">Запрос на создание кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task<long> AddLoanContract(LoanContract loanContractInternalRequest, CancellationToken token);

        /// <summary>
        /// Обновляет информацию о кредитном договоре.
        /// </summary>
        /// <param name="loanContract">Данные кредитного договора для обновления.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateLoanContract(LoanContract loanContract, CancellationToken token);
    }
}