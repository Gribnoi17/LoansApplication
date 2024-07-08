using System.Collections;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.UnitTests.Loans.Data
{
    internal class ValidLoanContractInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new LoanContractInternalRequest { ClientId = 1, Amount = 500000, LoanTermMonth = 2} };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 1, Amount = 20000, LoanTermMonth = 4 } };
            yield return new object[] { new LoanContractInternalRequest { ClientId = 2, Amount = 1000000, LoanTermMonth = 8} };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
