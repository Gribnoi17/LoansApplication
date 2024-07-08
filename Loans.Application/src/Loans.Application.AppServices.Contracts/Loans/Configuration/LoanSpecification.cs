namespace Loans.Application.AppServices.Contracts.Loans.Configuration
{
    /// <summary>
    /// Класс, представляющий спецификацию для кредита, включающую минимальные и максимальные значения для различных параметров.
    /// </summary>
    public class LoanSpecification
    {
        /// <summary>
        /// Минимальная сумма кредита.
        /// </summary>
        public decimal MinLoanAmount { get; set; }

        /// <summary>
        /// Максимальная сумма кредита.
        /// </summary>
        public decimal MaxLoanAmount { get; set; }

        /// <summary>
        /// Минимальный срок кредита в месяцах.
        /// </summary>
        public int MinLoanTermMonth { get; set; }

        /// <summary>
        /// Максимальный срок кредита в месяцах.
        /// </summary>
        public int MaxLoanTermMonth { get; set; }

        /// <summary>
        /// Минимальная зарплата для кредита.
        /// </summary>
        public decimal MinSalary { get; set; }
    }
}
