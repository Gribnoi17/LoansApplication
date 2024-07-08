using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Loans.Application.AppServices.Contracts.Loans.Configuration;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Contracts.Loans.Validators;
using Loans.Application.AppServices.Loans.Validators;
using Loans.Application.AppServices.UnitTests.Clients.Data;
using Loans.Application.AppServices.UnitTests.Loans.Data;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Loans.Validators
{
    public class LoanValidatorTests
    {
        private readonly IOptionsMonitor<LoanSpecification> _loanSpecification;
        private readonly IClientRepository _clientRepository;
        private readonly ILoanValidator _loanValidator;
        private readonly ClientTestData _clientTestData;
        
        public LoanValidatorTests()
        {
            _clientTestData = new ClientTestData();

            var mockLoanSpecificationOptions = Substitute.For<IOptionsMonitor<LoanSpecification>>();
            mockLoanSpecificationOptions.CurrentValue.Returns(new LoanSpecification
            {
                MinLoanAmount = 10000,
                MaxLoanAmount = 1000000,
                MinLoanTermMonth = 1,
                MaxLoanTermMonth = 30,
                MinSalary = 20000
            });
        
            _loanSpecification = mockLoanSpecificationOptions;
        
            _clientRepository = Substitute.For<IClientRepository>();

            _loanValidator = new LoanValidator(_loanSpecification, _clientRepository);
        }
        
        [Theory]
        [ClassData(typeof(ValidLoanContractInternalRequestTestData))]
        public void Validate_ValidLoanContractInput_NoExceptionThrown(LoanContractInternalRequest loanContractInternalRequest)
        {
            //Arrange - данные передаем в параметр
            var clients = _clientTestData.GetTestClients();
            var client = clients.FirstOrDefault(c => c.Id == loanContractInternalRequest.ClientId);
            
            _clientRepository.GetClientById(loanContractInternalRequest.ClientId, CancellationToken.None)!
                .Returns(client);
        
            //Act
            var exception = Record.Exception(() => _loanValidator.Validate(loanContractInternalRequest));
        
            //Assert
            Assert.Null(exception);
        }
        
        [Theory]
        [ClassData(typeof(InvalidLoanContractInternalRequestTestData))]
        public void Validate_InvalidLoanContractInput_ValidationExceptionThrown(LoanContractInternalRequest loanContractInternalRequest)
        {
            //Arrange - данные передаем в параметр
            var clients = _clientTestData.GetTestClients();
            var client = clients.FirstOrDefault(c => c.Id == loanContractInternalRequest.ClientId);
            
            _clientRepository.GetClientById(loanContractInternalRequest.ClientId, CancellationToken.None)!
                .Returns(client);
        
            //Act & Assert
            Assert.Throws<ValidationException>(() => _loanValidator.Validate(loanContractInternalRequest));
        }
    }
}