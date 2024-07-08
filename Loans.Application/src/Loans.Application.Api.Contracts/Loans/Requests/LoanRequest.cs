namespace Loans.Application.Api.Contracts.Loans.Requests
{
    /// <summary>
    /// Модель запроса для создания нового кредитного договора.
    /// </summary>
    public record LoanRequest
    {
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
        /// Зарплата клиента.
        /// </summary>
        public decimal Salary { get; set; }
    }
}
