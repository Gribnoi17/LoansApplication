namespace Loans.Application.AppServices.Contracts.Loans
{
    /// <summary>
    /// Возможные статусы кредитного договора.
    /// </summary>
    public enum LoanStatus
    {
        /// <summary>
        /// Статус неизвестен.
        /// </summary>
        Unknown,

        /// <summary>
        /// Кредитный договор находится в процессе обработки.
        /// </summary>
        InProgress,

        /// <summary>
        /// Кредитный договор одобрен.
        /// </summary>
        Approved,

        /// <summary>
        /// Кредитный договор отклонен.
        /// </summary>
        Denied
    }
}