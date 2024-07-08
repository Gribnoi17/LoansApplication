using System.Collections;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.UnitTests.Loans.Data
{
    public class ValidLoanContractTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new LoanContract{ClientId = 1, Amount = 500000, InterestRate = 10, LoanTermMonth = 30} };
            yield return new object[] { new LoanContract{ClientId = 2, Amount = 300000, InterestRate = 12, LoanTermMonth = 20} };
            yield return new object[] { new LoanContract{ClientId = 3, Amount = 200000, InterestRate = 8, LoanTermMonth = 15} };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}