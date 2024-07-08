using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.DataAccess.Loans.Models;

namespace Loans.Application.DataAccess.UnitTests.Loans.Data
{
    internal class LoanContractEntityTestData
    {
        private List<LoanContractEntity> _testLoanContractEntities;

        public List<LoanContractEntity> GetTestLoanContractEntities()
        {
            if (_testLoanContractEntities == null)
            {
                _testLoanContractEntities = InitializeLoanContractEntitiesTestData();
            }

            return _testLoanContractEntities.ToList();
        }

        private List<LoanContractEntity> InitializeLoanContractEntitiesTestData()
        {
            var loanContractEntities = new List<LoanContractEntity>();

            loanContractEntities.Add(new LoanContractEntity
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                ExpectedInterestRate = 10,
                LoanTermMonth = 30,
                LoanDate = new DateOnly(2020, 04, 13),
                Status = LoanStatus.Approved
            });

            loanContractEntities.Add(new LoanContractEntity
            {
                Id = 2,
                ClientId = 2,
                Amount = 500000,
                ExpectedInterestRate = 8,
                LoanTermMonth = 25,
                LoanDate = new DateOnly(2022, 06, 13),
                RejectionReason = "Что-то",
                Status = LoanStatus.Denied
            });

            loanContractEntities.Add(new LoanContractEntity
            {
                Id = 3,
                ClientId = 2,
                Amount = 66666,
                ExpectedInterestRate = 18,
                LoanTermMonth = 14,
                LoanDate = new DateOnly(2023, 02, 02),
                Status = LoanStatus.InProgress
            });


            return loanContractEntities;
        }
    }
}