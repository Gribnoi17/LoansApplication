namespace Loans.Application.AppServices.Contracts.Loans.Handlers
{
    /// <summary>
    /// Обработчик для получения статуса кредитного договора по его идентификатору.
    /// </summary>
    public interface IGetLoanContractStatusHandler
    {
        /// <summary>
        /// Обрабатывает запрос на получение статуса кредитного договора по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор кредитного договора.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Статус кредитного договора.</returns>
        Task<LoanStatus> Handle(long id, CancellationToken token);
    }
}
