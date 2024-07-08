using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Handlers;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.UnitTests.Clients.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using LoanStatus = Loans.Application.AppServices.Contracts.Loans.LoanStatus;

namespace Loans.Application.AppServices.UnitTests.Loans.Handlers
{
    public class GetLoanContractStatusHandlerTests
    {
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly IGetLoanContractStatusHandler _getLoanContractStatusHandler;
        private readonly ILogger<GetLoanContractStatusHandler> _logger;
        private readonly ClientTestData _clientTestData;
        
        public GetLoanContractStatusHandlerTests()
        {
            _clientTestData = new ClientTestData();

            _loanContractRepository = Substitute.For<ILoanContractRepository>();
            _logger = Substitute.For<ILogger<GetLoanContractStatusHandler>>();

            _getLoanContractStatusHandler = new GetLoanContractStatusHandler(_loanContractRepository, _logger);
        }
        
        [Fact]
        public async Task Handle_LoanWithStatusApproved_ReturnsStatusApproved()
        {
            // Arrange
            var loan = new LoanContract{ClientId = 1, Amount = 10000, InterestRate = 10, LoanTermMonth = 2};
            loan.Status = LoanStatus.Approved;
            loan.Id = 1;

            _loanContractRepository.GetLoanContractById(loan.Id, CancellationToken.None).Returns(loan);
        
            // Act
            var status = await _getLoanContractStatusHandler.Handle(loan.Id, CancellationToken.None);
        
            // Assert
            Assert.Equal(LoanStatus.Approved, status);
        }
        
        [Fact]
        public async Task Handle_LoanWithStatusDenied_ReturnsStatusDenied()
        {
            // Arrange
            var loan = new LoanContract{ClientId = 1, Amount = 10000, InterestRate = 10, LoanTermMonth = 2};
            loan.Status = LoanStatus.Denied;
            loan.Id = 1;
            
            _loanContractRepository.GetLoanContractById(loan.Id, CancellationToken.None).Returns(loan);
        
            // Act
            var status = await _getLoanContractStatusHandler.Handle(loan.Id, CancellationToken.None);
        
            // Assert
            Assert.Equal(LoanStatus.Denied, status);
        }
    }
}