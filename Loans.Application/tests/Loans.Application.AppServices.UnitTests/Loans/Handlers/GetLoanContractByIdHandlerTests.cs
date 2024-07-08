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

    public class GetLoanContractByIdHandlerTests
    {
        private readonly ILoanContractRepository _loanContractRepository;

        private readonly IGetLoanContractByIdHandler _loanContractByIdHandler;
        private readonly ILogger<GetLoanContractByIdHandler> _logger;

        public GetLoanContractByIdHandlerTests()
        {
            _loanContractRepository = Substitute.For<ILoanContractRepository>();
            _logger = Substitute.For<ILogger<GetLoanContractByIdHandler>>();

            _loanContractByIdHandler = new GetLoanContractByIdHandler(_loanContractRepository, _logger);
        }

        [Theory]
        [ClassData(typeof(ValidLoanContractTestData))]
        public async Task Handle_WithValidId_ReturnsLoanContract(LoanContract loanContract)
        {
            //Arange
            loanContract.Id = 1;
            _loanContractRepository.GetLoanContractById(loanContract.Id, CancellationToken.None)
                .Returns(loanContract);

            //Act
            var result = await _loanContractByIdHandler.Handle(loanContract.Id, CancellationToken.None);

            //Assert
            Assert.Equal(loanContract, result);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ThrowsInvalidOperationException()
        {
            //Arange
            var loanContract = new LoanContract{ClientId = 1, Amount = 500000, InterestRate = 10, LoanTermMonth = 30};
            loanContract.Id = 1;

            _loanContractRepository.GetLoanContractById(Arg.Any<long>(), CancellationToken.None)
                .Throws(new InvalidOperationException("Договор кредита с таким id не существует!"));

            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _loanContractByIdHandler.Handle(loanContract.Id, CancellationToken.None));
        }
    }
}