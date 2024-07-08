using DCS.DecisionMakerService.Client.Kafka.Enums;
using DCS.DecisionMakerService.Client.Kafka.Events;
using DCS.DecisionMakerService.Client.Kafka.Models;
using KafkaFlow;
using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.Host.Infrastructure.Kafka.Consumers;
using Microsoft.Extensions.Logging;
using Xunit;
using NSubstitute;

namespace Loans.Application.Host.UnitTests.Infrastructure.Kafka.Consumers
{
    public class CalculateDecisionEventHandlerTests
    {
        private readonly IProcessLoanContractDecisionHandler _processLoanContractDecisionHandler;
        private readonly IMessageContext _messageContext;
        private readonly ILogger<CalculateDecisionEventHandler> _logger;

        private readonly CalculateDecisionEventHandler _calculateDecisionEventHandler;

        public CalculateDecisionEventHandlerTests()
        {
            _processLoanContractDecisionHandler = Substitute.For<IProcessLoanContractDecisionHandler>();
            _messageContext = Substitute.For<IMessageContext>();
            _logger = Substitute.For<ILogger<CalculateDecisionEventHandler>>();

            _calculateDecisionEventHandler = new CalculateDecisionEventHandler(_processLoanContractDecisionHandler, _logger);
        }

        [Fact]
        public async Task Handle_WithValidResponse_SendsCorrectDataToProcessLoanContractDecisionHandler()
        {
            //Arrange
            var response = new CalculateDecisionEventResult
            {
                ApplicationId = 2,
                ClientId = 10,
                Decision = new Decision
                {
                    LoanOffer = new LoanOffer
                    {
                        AnnuityAmount = 30000,
                        CreditAmount = 95000,
                        CreditLenMonth = 12,
                        InterestRate = 9
                    }
                }
            };

            //Act
            await _calculateDecisionEventHandler.Handle(_messageContext, response);
            
            //Assert
            await _processLoanContractDecisionHandler.Received(1).Handle(Arg.Any<LoanContractEventResult>(), Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public void ToLoanContractEventResult_WithValidData_MapsCorrectly()
        {
            //Arrange
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = 2,
                Amount = 95000,
                ClientId = 10,
                InterestRate = 9,
                LoanDate = _messageContext.ConsumerContext.MessageTimestamp,
                LoanTermMonth = 12,
                Status = LoanStatus.Approved
            };
            
            var response = new CalculateDecisionEventResult
            {
                ApplicationId = 2,
                ClientId = 10,
                Decision = new Decision
                {
                    LoanOffer = new LoanOffer
                    {
                        AnnuityAmount = 30000,
                        CreditAmount = 95000,
                        CreditLenMonth = 12,
                        InterestRate = 9
                    },
                    DecisionStatus = DecisionStatus.Approval
                }
            };

            //Act
            var result = _calculateDecisionEventHandler.ToLoanContractEventResult(_messageContext, response);
            
            //Assert
            Assert.Equal(loanContractEventResult.LoanContractId, result.LoanContractId);
            Assert.Equal(loanContractEventResult.ClientId, result.ClientId);
            Assert.Equal(loanContractEventResult.Amount, result.Amount);
            Assert.Equal(loanContractEventResult.InterestRate, result.InterestRate);
            Assert.Equal(loanContractEventResult.LoanDate, result.LoanDate);
            Assert.Equal(loanContractEventResult.LoanTermMonth, result.LoanTermMonth);
            Assert.Equal(loanContractEventResult.Status, result.Status);
        }
        
        [Fact]
        public void MapStatus_WithApprovedStatus_StatusHasBecomeApproved()
        {
            //Arrange
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = 2,
                Amount = 95000,
                ClientId = 10,
                InterestRate = 9,
                LoanDate = _messageContext.ConsumerContext.MessageTimestamp,
                LoanTermMonth = 12,
            };

            //Act
            _calculateDecisionEventHandler.MapStatus(loanContractEventResult, DecisionStatus.Approval);
            
            //Assert
            Assert.Equal(LoanStatus.Approved, loanContractEventResult.Status);
        }
        
        [Fact]
        public void MapStatus_WithUnderwritingStatus_StatusHasBecomeInProgress()
        {
            //Arrange
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = 2,
                Amount = 95000,
                ClientId = 10,
                InterestRate = 9,
                LoanDate = _messageContext.ConsumerContext.MessageTimestamp,
                LoanTermMonth = 12,
            };

            //Act
            _calculateDecisionEventHandler.MapStatus(loanContractEventResult, DecisionStatus.Underwriting);
            
            //Assert
            Assert.Equal(LoanStatus.InProgress, loanContractEventResult.Status);
        }
        
        [Fact]
        public void MapStatus_WithRefuseStatus_StatusHasBecomeDenied()
        {
            //Arrange
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = 2,
                Amount = 95000,
                ClientId = 10,
                InterestRate = 9,
                LoanDate = _messageContext.ConsumerContext.MessageTimestamp,
                LoanTermMonth = 12,
            };

            //Act
            _calculateDecisionEventHandler.MapStatus(loanContractEventResult, DecisionStatus.Refuse);
            
            //Assert
            Assert.Equal(LoanStatus.Denied, loanContractEventResult.Status);
        }
        
        [Fact]
        public void MapStatus_WithUnknownStatus_StatusHasBecomeUnknown()
        {
            //Arrange
            var loanContractEventResult = new LoanContractEventResult
            {
                LoanContractId = 2,
                Amount = 95000,
                ClientId = 10,
                InterestRate = 9,
                LoanDate = _messageContext.ConsumerContext.MessageTimestamp,
                LoanTermMonth = 12,
            };

            //Act
            _calculateDecisionEventHandler.MapStatus(loanContractEventResult, DecisionStatus.Unknown);
            
            //Assert
            Assert.Equal(LoanStatus.Unknown, loanContractEventResult.Status);
        }
    }
}