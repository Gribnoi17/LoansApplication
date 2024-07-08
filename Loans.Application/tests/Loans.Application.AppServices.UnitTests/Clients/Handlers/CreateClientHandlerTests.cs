using Loans.Application.AppServices.Clients.Handlers;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Clients.Validators;
using Loans.Application.AppServices.UnitTests.Clients.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Clients.Handlers
{
    public class CreateClientHandlerTests
    {
        private readonly IClientValidator _clientValidator;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<CreateClientHandler> _logger;
        
        private readonly ICreateClientHandler _createClientHandler;

        public CreateClientHandlerTests()
        {
            _clientValidator = Substitute.For<IClientValidator>();
            _clientRepository = Substitute.For<IClientRepository>();
            _logger = Substitute.For<ILogger<CreateClientHandler>>();
            
            _createClientHandler = new CreateClientHandler(_clientValidator, _clientRepository, _logger);
        }
        
        [Theory]
        [ClassData(typeof(ValidClientInternalRequestTestData))]
        public async Task Handle_SuccessfulClientCreation_ReturnsClientId(ClientInternalRequest clientInternalRequest)
        {
            //Arrange
            long nextId = 0;
            
            var client = new Client
            {
                FirstName = clientInternalRequest.FirstName,
                LastName = clientInternalRequest.LastName,
                MiddleName = clientInternalRequest.MiddleName,
                BirthDate = clientInternalRequest.BirthDate,
            };

            _clientRepository.AddClient(Arg.Any<Client>(), CancellationToken.None).Returns(x => 
            {
                var clientArgument = x.Arg<Client>();
                clientArgument.Id = ++nextId;
                client.Id = nextId;
                return clientArgument.Id;
            });
            
            //Act
            var result = await _createClientHandler.Handle(clientInternalRequest, CancellationToken.None);
            
            // Assert
            Assert.Equal(client.Id, result.Id);
            Assert.Equal(client.FirstName, result.FirstName);
            Assert.Equal(client.LastName, result.LastName);
            Assert.Equal(client.MiddleName, result.MiddleName);
            Assert.Equal(client.BirthDate, result.BirthDate);
            Assert.Equal(client.Salary, result.Salary);
        }
    }
}
