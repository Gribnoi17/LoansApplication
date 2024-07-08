using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Handlers;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.UnitTests.Loans.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Loans.Handlers
{
    public class ProcessLoanContractDecisionHandlerTests
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly IProcessLoanContractDecisionHandler _processLoanContractDecisionHandler;
        private readonly ILogger<ProcessLoanContractDecisionHandler> _logger;
        
        public ProcessLoanContractDecisionHandlerTests()
        {
            _loanContractRepository = Substitute.For<ILoanContractRepository>();
            _logger = Substitute.For<ILogger<ProcessLoanContractDecisionHandler>>();
            
            _processLoanContractDecisionHandler = new ProcessLoanContractDecisionHandler(_loanContractRepository, _logger);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public void ApplyDecisionToLoanContract_WithApprovedStatus_ReturnLoanContractWithStatusApproved(LoanContract loanContract)
        {
            //A
            var eventResult = new LoanContractEventResult
            {
                LoanContractId = 1,
                ClientId = 2,
                LoanTermMonth = 12,
                Amount = 50000,
                InterestRate = 12,
                LoanDate = new DateTime(2023, 12, 12),
                Status = LoanStatus.Approved
            };

            //Act
            ProcessLoanContractDecisionHandler.ApplyDecisionToLoanContract(loanContract, eventResult, "Отказ");

            //Assert
            Assert.Equal(eventResult.LoanContractId, loanContract.Id);
            Assert.Equal(eventResult.ClientId, loanContract.ClientId);
            Assert.Equal(eventResult.LoanTermMonth, loanContract.LoanTermMonth);
            Assert.Equal(eventResult.InterestRate, loanContract.InterestRate);
            Assert.Equal(eventResult.LoanDate, loanContract.LoanDate);
            Assert.Equal(eventResult.Status, loanContract.Status);
            Assert.True(loanContract.RejectionReason == null);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public void ApplyDecisionToLoanContract_WithDeniedStatus_ReturnLoanContractWithStatusDenied(LoanContract loanContract)
        {
            //Arrange
            var eventResult = new LoanContractEventResult
            {
                LoanContractId = 1,
                ClientId = 2,
                LoanTermMonth = 12,
                Amount = 50000,
                InterestRate = 12,
                LoanDate = new DateTime(2023, 12, 12),
                Status = LoanStatus.Denied
            };

            //Act
            ProcessLoanContractDecisionHandler.ApplyDecisionToLoanContract(loanContract, eventResult, "Отказ");

            //Assert
            Assert.Equal(eventResult.LoanContractId, loanContract.Id);
            Assert.Equal(eventResult.ClientId, loanContract.ClientId);
            Assert.Equal(eventResult.LoanTermMonth, loanContract.LoanTermMonth);
            Assert.Equal(eventResult.InterestRate, loanContract.InterestRate);
            Assert.Equal(eventResult.LoanDate, loanContract.LoanDate);
            Assert.Equal(eventResult.Status, loanContract.Status);
            Assert.Equal("Отказ", loanContract.RejectionReason);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public void ApplyDecisionToLoanContract_WithDeniedStatus_ReturnLoanContractWithStatusInProgress(LoanContract loanContract)
        {
            //Arrange
            var eventResult = new LoanContractEventResult
            {
                LoanContractId = 1,
                ClientId = 2,
                LoanTermMonth = 12,
                Amount = 50000,
                InterestRate = 12,
                LoanDate = new DateTime(2023, 12, 12),
                Status = LoanStatus.InProgress
            };

            //Act
            ProcessLoanContractDecisionHandler.ApplyDecisionToLoanContract(loanContract, eventResult, "Отказ");

            //Assert
            Assert.Equal(eventResult.LoanContractId, loanContract.Id);
            Assert.Equal(eventResult.ClientId, loanContract.ClientId);
            Assert.Equal(eventResult.LoanTermMonth, loanContract.LoanTermMonth);
            Assert.Equal(eventResult.InterestRate, loanContract.InterestRate);
            Assert.Equal(eventResult.LoanDate, loanContract.LoanDate);
            Assert.Equal(eventResult.Status, loanContract.Status);
            Assert.True(loanContract.RejectionReason == null);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public void ApplyDecisionToLoanContract_WithDeniedStatus_ReturnLoanContractWithStatusUnknown(LoanContract loanContract)
        {
            //Arrange
            var eventResult = new LoanContractEventResult
            {
                LoanContractId = 1,
                ClientId = 2,
                LoanTermMonth = 12,
                Amount = 50000,
                InterestRate = 12,
                LoanDate = new DateTime(2023, 12, 12),
                Status = LoanStatus.Unknown
            };

            //Act
            ProcessLoanContractDecisionHandler.ApplyDecisionToLoanContract(loanContract, eventResult, "Отказ");

            //Assert
            Assert.Equal(eventResult.LoanContractId, loanContract.Id);
            Assert.Equal(eventResult.ClientId, loanContract.ClientId);
            Assert.Equal(eventResult.LoanTermMonth, loanContract.LoanTermMonth);
            Assert.Equal(eventResult.InterestRate, loanContract.InterestRate);
            Assert.Equal(eventResult.LoanDate, loanContract.LoanDate);
            Assert.Equal(eventResult.Status, loanContract.Status);
            Assert.Equal("Отказ", loanContract.RejectionReason);
        }

        [Fact]
        public async Task Handle_WithApprovedStatus_UpdateLoanContractCalledWithCorrectParameters()
        {
            // Arrange
            var eventResult = new LoanContractEventResult
            {
                LoanContractId = 1,
                ClientId = 2,
                LoanTermMonth = 12,
                Amount = 50000,
                InterestRate = 12,
                LoanDate = new DateTime(2023, 12, 12),
                Status = LoanStatus.Approved
            };

            _loanContractRepository.UpdateLoanContract(Arg.Any<LoanContract>(), CancellationToken.None).Returns(Task.CompletedTask);

            // Act
            await _processLoanContractDecisionHandler.Handle(eventResult, CancellationToken.None);

            // Assert
            await _loanContractRepository.Received(1).UpdateLoanContract(Arg.Is<LoanContract>(
                lc => lc.Id == eventResult.LoanContractId &&
                      lc.ClientId == eventResult.ClientId &&
                      lc.LoanTermMonth == eventResult.LoanTermMonth &&
                      lc.Amount == eventResult.Amount &&
                      lc.InterestRate == eventResult.InterestRate &&
                      lc.LoanDate == eventResult.LoanDate &&
                      lc.Status == eventResult.Status
            ), Arg.Any<CancellationToken>());
        }
    }
}