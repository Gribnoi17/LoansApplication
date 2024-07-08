namespace Loans.Application.AppServices.Contracts.Loans.Models
{
    /// <summary>
    /// Представляет входные данные для создания кредитного договора.
    /// </summary>
    public class LoanContractInternalRequest
    {
        /// <summary>
        /// Идентификатор клиента, для которого создается кредитный договор.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Желаемая сумма кредита.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Срок кредита в месяцах.
        /// </summary>
        public int LoanTermMonth { get; set; }
        
        /// <summary>
        /// Зарплата клиента.
        /// </summary>
        public decimal Salary { get; set; }
    }
}