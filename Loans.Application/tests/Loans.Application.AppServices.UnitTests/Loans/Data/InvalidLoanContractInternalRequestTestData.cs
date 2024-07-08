using System.Collections;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.UnitTests.Loans.Data
{
    internal class InvalidLoanContractInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new LoanContractInternalRequest { ClientId = 30, Amount = 300000, LoanTermMonth = 2} };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 2, Amount = 100, LoanTermMonth = 4} };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 2, Amount = 5000000, LoanTermMonth = 4 } };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 2, Amount = 60000, LoanTermMonth = 0} };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 1, Amount = 10000, LoanTermMonth = 130} };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
