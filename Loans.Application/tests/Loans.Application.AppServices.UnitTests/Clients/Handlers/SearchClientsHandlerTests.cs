using Loans.Application.AppServices.Clients.Handlers;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Clients.Handlers
{
    public class SearchClientsHandlerTests
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<SearchClientsHandler> _logger;

        private readonly ISearchClientsHandler _searchClientsHandler;

        public SearchClientsHandlerTests()
        {
            _clientRepository = Substitute.For<IClientRepository>();
            _logger = Substitute.For<ILogger<SearchClientsHandler>>();

            _searchClientsHandler = new SearchClientsHandler(_clientRepository, _logger);
        }

        [Fact]
        public async Task Handle_WithValidFilter_ReturnsClients()
        {
            //Arrange
            var clients = new List<Client>();

            clients.Add(new Client { FirstName = "Данил", LastName = "Китов", BirthDate = new DateTime(2000, 01, 04), Salary = 120000 });
            clients.Add(new Client { FirstName = "Данил", LastName = "Морозов", MiddleName = "Денисович", BirthDate = new DateTime(2001, 06, 09), Salary = 180000 });

            var filter = new ClientInternalFilter
            {
                FirstName = "Данил"
            };

            _clientRepository.SearchClients(filter, CancellationToken.None).Returns(clients);

            //Act
            var result = await _searchClientsHandler.Handle(filter, CancellationToken.None);

            //Assert
            Assert.Equal(clients, result);
        }

        [Fact]
        public async Task Handle_WithInvalidFilter_ReturnsEmptyClientsList()
        {
            //Arrange
            var clients = new List<Client>();

            var filter = new ClientInternalFilter
            {
                FirstName = "Дима"
            };

            _clientRepository.SearchClients(filter, CancellationToken.None).Returns(clients);

            //Act
            var result = await _searchClientsHandler.Handle(filter, CancellationToken.None);

            //Assert
            Assert.Empty(result);
        }
    }
}