namespace Loans.Application.AppServices.Contracts.Loans.Models
{
    /// <summary>
    /// Представляет кредитный договор.
    /// </summary>
    public class LoanContract
    {
        /// <summary>
        /// Уникальный идентификатор кредитного договора.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор клиента, связанного с кредитным договором.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Сумма кредита.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Срок кредита в месяцах.
        /// </summary>
        public int LoanTermMonth { get; set; }

        /// <summary>
        /// Процентная ставка по кредиту.
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Дата заключения кредитного договора.
        /// </summary>
        public DateTime LoanDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Статус кредитного договора.
        /// </summary>
        public LoanStatus Status { get; set; } = LoanStatus.InProgress;

        /// <summary>
        /// Причина отказа в случае, если статус договора - отклонен.
        /// </summary>
        public string? RejectionReason { get; set; }
    }
}