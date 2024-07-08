using System.Collections;
using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.DataAccess.UnitTests.Loans.Data
{
    internal class LoanContractTestData : IEnumerable<object[]>
    {
        private readonly LoanContract _loanContractOne;
        private readonly LoanContract _loanContractTwo; 
        private readonly LoanContract _loanContractThree;

        public LoanContractTestData()
        {
            _loanContractOne = new LoanContract
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                InterestRate = 10,
                LoanTermMonth = 30,
                LoanDate = new DateTime(2020, 04, 13),
                Status = LoanStatus.Approved
            };
            
            _loanContractTwo = new LoanContract
            {
                Id = 2,
                ClientId = 2,
                Amount = 500000,
                InterestRate = 8,
                LoanTermMonth = 25,
                LoanDate = new DateTime(2022, 06, 13),
                RejectionReason = "Что-то",
                Status = LoanStatus.Denied
            };
            
            _loanContractThree = new LoanContract
            {
                Id = 3,
                ClientId = 2,
                Amount = 66666,
                InterestRate = 18,
                LoanTermMonth = 14,
                LoanDate = new DateTime(2023, 02, 02),
                Status = LoanStatus.InProgress
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { _loanContractOne};
            yield return new object[] { _loanContractTwo };
            yield return new object[] { _loanContractThree};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<LoanContract> GetLoanContracts()
        {
            var loanContracts = new List<LoanContract>()
            {
                _loanContractOne,
                _loanContractTwo,
                _loanContractThree
            };

            return loanContracts;
        }
    }
}