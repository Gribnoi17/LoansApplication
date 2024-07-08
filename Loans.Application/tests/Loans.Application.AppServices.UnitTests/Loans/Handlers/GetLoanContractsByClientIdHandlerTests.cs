using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Handlers;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.UnitTests.Loans.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Loans.Handlers
{
    public class GetLoanContractsByClientIdHandlerTests
    {
        private readonly ILoanContractRepository _loanContractRepository;

        private readonly IGetLoanContractsByClientIdHandler _loanContractsByClientIdHandler;
        private readonly ILogger<GetLoanContractsByClientIdHandler> _logger;

        public GetLoanContractsByClientIdHandlerTests()
        {
            _loanContractRepository = Substitute.For<ILoanContractRepository>();
            _logger = Substitute.For<ILogger<GetLoanContractsByClientIdHandler>>();

            _loanContractsByClientIdHandler = new GetLoanContractsByClientIdHandler(_loanContractRepository, _logger);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public async Task Handle_WithValidClientId_ReturnsLoanContract(LoanContract loanContract)
        {
            //Arange
            var loanContracts = new List<LoanContract> { loanContract };
            
            _loanContractRepository.GetLoanContractsByClientId(loanContract.ClientId, CancellationToken.None)
                .Returns(loanContracts);
            
            //Act
            var result = await _loanContractsByClientIdHandler.Handle(loanContract.ClientId, CancellationToken.None);
            
            //Assert
            Assert.Equal(loanContracts, result);
        }
        
        [Fact]
        public async Task Handle_WithInvalidId_ThrowsInvalidOperationException()
        {
            //Arange
            var loanContract = new LoanContract{ClientId = 1, Amount = 500000, InterestRate = 10, LoanTermMonth = 30};

            _loanContractRepository.GetLoanContractsByClientId(Arg.Any<long>(), CancellationToken.None)
                .Throws(new InvalidOperationException("Договор кредита с таким id клиента не существует!"));
            
            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>( ()=> _loanContractsByClientIdHandler.Handle(loanContract.ClientId, CancellationToken.None));
        }
    }
}