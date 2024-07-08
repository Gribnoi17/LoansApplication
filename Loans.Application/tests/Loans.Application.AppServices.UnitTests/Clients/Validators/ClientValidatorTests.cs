using Loans.Application.AppServices.Clients.Validators;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Clients.Validators;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Loans.Application.AppServices.UnitTests.Clients.Data;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Clients.Validators
{
    public class ClientValidatorTests
    {
        private IClientValidator _clientValidator;

        public ClientValidatorTests()
        {
            _clientValidator = new ClientValidator();
        }

        [Theory]
        [ClassData(typeof(ValidClientInternalRequestTestData))]
        public void Validate_ValidClientInput_NoExceptionThrown(ClientInternalRequest clientInternalRequest)
        {
            //Arrange - данные передаем в параметр

            //Act
            var exception = Record.Exception(() => _clientValidator.Validate(clientInternalRequest));

            //Assert
            Assert.Null(exception);
        }

        [Theory]
        [ClassData(typeof(InvalidClientInternalRequestTestData))]
        public void Validate_InvalidClientInput_ValidationExceptionThrown(ClientInternalRequest clientInternalRequest)
        {
            //Arrange - данные передаем в параметр

            //Act & Assert
            Assert.Throws<ValidationException>(() => _clientValidator.Validate(clientInternalRequest));
        }

        [Fact]
        public void IsAdult_ClientIsAdult_ReturnTrue()
        {
            //Arrange
            var dateOfBirth = new DateTime(2005, 10, 8);
            var currentDate = new DateTime(2023, 10, 17);

            // Assert & Act
            Assert.True(ClientValidator.IsAdult(dateOfBirth, currentDate));
        }

        [Fact]
        public void IsAdult_ClientIsNotAdult_ReturnsFalse()
        {
            //Arrange
            var dateOfBirth = new DateTime(2005, 10, 18);
            var currentDate = new DateTime(2023, 10, 17);

            // Assert & Act
            Assert.False(ClientValidator.IsAdult(dateOfBirth, currentDate));
        }
    }
}