using Loans.Application.Api.Contracts.Loans.Enum;

namespace Loans.Application.Api.Contracts.Loans.Responses
{
    /// <summary>
    /// Модель ответа для представления информации о кредитном договоре.
    /// </summary>
    public class LoanContractResponse
    {
        /// <summary>
        /// Идентификатор кредитного договора.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Сумма кредита.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Срок кредита в месяцах.
        /// </summary>
        public int LoanTermMonth { get; set; }

        /// <summary>
        /// Процентная ставка.
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Причина отказа.
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// Дата создания кредитного договора.
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Статус кредитного договора.
        /// </summary>
        public LoanStatus Status { get; set; }
    }
}