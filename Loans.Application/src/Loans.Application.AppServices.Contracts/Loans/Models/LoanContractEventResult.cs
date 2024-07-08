namespace Loans.Application.AppServices.Contracts.Loans.Models
{
    public class LoanContractEventResult
    {
        /// <summary>
        /// Идентификатор заявки.
        /// </summary>
        public long LoanContractId { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public long ClientId { get; set; }
        
        /// <summary>
        /// Срок кредита в месяцах.
        /// </summary>
        public int LoanTermMonth { get; set; }
        
        /// <summary>
        /// Сумма кредита.
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Процентная ставка по кредиту.
        /// </summary>
        public decimal InterestRate { get; set; }
        
        /// <summary>
        /// Дата заключения кредитного договора.
        /// </summary>
        public DateTime LoanDate { get; set; }
        
        /// <summary>
        /// Статус кредитного договора.
        /// </summary>
        public LoanStatus Status { get; set; }
    }
}