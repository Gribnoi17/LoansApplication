using Loans.Application.AppServices.Clients.Handlers;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Clients.Handlers
{
    public class UpdateClientHandlerTests
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<UpdateClientHandler> _logger;
        
        private readonly IUpdateClientHandler _updateClientHandler;

        public UpdateClientHandlerTests()
        {
            _clientRepository = Substitute.For<IClientRepository>();
            _logger = Substitute.For<ILogger<UpdateClientHandler>>();

            _updateClientHandler = new UpdateClientHandler(_clientRepository, _logger);
        }

        [Fact]
        public async Task Handle_ValidClientUpdate_ReturnsUpdatedClient()
        {
            //Arrange
            var request = new ClientUpdateInternalRequest
            {
                Id = 10,
                FirstName = "Новое имя",
                LastName = "Новая фамилия",
                MiddleName = "Новое отчество",
                Salary = 50000
            };
            
            var client = new Client
            {
                Id = 10,
                FirstName = "Данил",
                LastName = "Китов",
                MiddleName = "",
                BirthDate = new DateTime(2000, 01, 04),
                Salary = 120000
            };

            _clientRepository.GetClientById(request.Id, CancellationToken.None).Returns(client);
            _clientRepository.UpdateClient(client, CancellationToken.None).Returns(Task.CompletedTask);
            
            //Act
            await _updateClientHandler.Handle(request, CancellationToken.None);
            
            // Assert
            Assert.Equal(request.Id, client.Id);
            Assert.Equal(request.FirstName, client.FirstName);
            Assert.Equal(request.LastName, client.LastName);
            Assert.Equal(request.MiddleName, client.MiddleName);
            Assert.Equal(request.Salary, client.Salary);
        }
        
        [Fact]
        public async Task Handle_InvalidClientUpdate_ThrowsValidationException()
        {
            //Arrange
            var request = new ClientUpdateInternalRequest
            {
                Id = 10,
                Salary = -10000
            };
            
            var client = new Client
            {
                Id = 10,
                FirstName = "Данил",
                LastName = "Китов",
                MiddleName = "",
                BirthDate = new DateTime(2000, 01, 04),
                Salary = 120000
            };

            _clientRepository.GetClientById(request.Id, CancellationToken.None).Returns(client);
            _clientRepository.UpdateClient(client, CancellationToken.None).Returns(Task.CompletedTask);
            
            //Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _updateClientHandler.Handle(request, CancellationToken.None));
        }
    }
}