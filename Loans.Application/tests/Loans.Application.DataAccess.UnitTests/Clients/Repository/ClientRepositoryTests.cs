using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.DataAccess.Clients.Models;
using Loans.Application.DataAccess.Clients.Repository;
using Loans.Application.DataAccess.Data;
using Loans.Application.DataAccess.UnitTests.Clients.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace Loans.Application.DataAccess.UnitTests.Clients.Repository
{
    public class ClientRepositoryTests
    {
        private readonly DbContextOptions<LoansDbContext> _dbContextOptions;
        private readonly ClientEntityTestData _clientEntityTestData;
        private readonly ClientTestData _clientTestData;

        public ClientRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<LoansDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseForClient")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _clientEntityTestData = new ClientEntityTestData();
            _clientTestData = new ClientTestData();
            
            using (var context = new LoansDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task GetClientById_WithValidId_ReturnsClient()
        {
            //Arrange
            var clientEntity = new ClientEntity
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateOnly(2002, 06, 01),
                Salary = 50000
            };

            var client = new Client
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateTime(2002, 06, 01),
                Salary = 50000
            };
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                await context.Clients.AddAsync(clientEntity);
                await context.SaveChangesAsync();
            }

            Client result;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);

                // Act
                result = await clientRepository.GetClientById(clientEntity.Id, CancellationToken.None);
            }
            
            // Assert
            Assert.Equal(client.FirstName, result.FirstName);
            Assert.Equal(client.LastName, result.LastName);
            Assert.Equal(client.MiddleName, result.MiddleName);
            Assert.Equal(client.BirthDate.Date, result.BirthDate);
            Assert.Equal(client.Salary, result.Salary);
        }

        [Fact]
        public async Task GetClientById_WithInvalidId_ThrowsInvalidOperationException()
        {
            //Arrange
            long id = -1;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    clientRepository.GetClientById(id, CancellationToken.None));
            }
        }
        
        [Fact]
        public void GetClientById_WithCanceledToken_ReturnsCanceledTask()
        {
            // Arrange
            long validId = 1;
            var canceledToken = new CancellationToken(canceled: true);

            Task task;
            
            using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                task = clientRepository.GetClientById(validId, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task SearchClients_WithValidFilter_ReturnsClients()
        {
            //Arrange
            var expectedClients = _clientTestData.GetTestClients();
            expectedClients.RemoveRange(2, expectedClients.Count - 2);
            
            var clientEntities = _clientEntityTestData.GetTestClientEntities();

            var filter = new ClientInternalFilter
            {
                FirstName = "Данил"
            };

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                foreach (var client in clientEntities)
                {
                    await context.Clients.AddAsync(client);
                }
                await context.SaveChangesAsync();
            }

            List<Client> result;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                result = await clientRepository.SearchClients(filter, CancellationToken.None);
            }

            //Assert
            Assert.Equal(expectedClients.Count, result.Count);
            
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedClients[i].FirstName, result[i].FirstName);
                Assert.Equal(expectedClients[i].LastName, result[i].LastName);
                Assert.Equal(expectedClients[i].MiddleName, result[i].MiddleName);
                Assert.Equal(expectedClients[i].BirthDate.Date, result[i].BirthDate.Date);
                Assert.Equal(expectedClients[i].Salary, result[i].Salary);
            }
        }

        [Fact]
        public async Task SearchClients_ByAllFilter_ReturnsClients()
        {
            // Arrange
            var clients = _clientTestData.GetTestClients();
            var expectedClients = new List<Client>{clients[1]};
            
            var clientEntities = _clientEntityTestData.GetTestClientEntities();

            var filter = new ClientInternalFilter
            {
                FirstName = "Данил",
                LastName = "Петров",
                MiddleName = "Кириллович",
                BirthDate = new DateTime(1999, 02, 01)
            };
        
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                foreach (var client in clientEntities)
                {
                    await context.Clients.AddAsync(client);
                }
                await context.SaveChangesAsync();
            }

            List<Client> result;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                result = await clientRepository.SearchClients(filter, CancellationToken.None);
            }
            
            // Assert
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedClients[i].FirstName, result[i].FirstName);
                Assert.Equal(expectedClients[i].LastName, result[i].LastName);
                Assert.Equal(expectedClients[i].MiddleName, result[i].MiddleName);
                Assert.Equal(expectedClients[i].BirthDate.Date, result[i].BirthDate.Date);
                Assert.Equal(expectedClients[i].Salary, result[i].Salary);
            }
        }
        
        [Fact]
        public async Task SearchClients_WithInvalidFilter_ReturnsEmptyClientsList()
        {
            //Arrange
            var clientEntities = _clientEntityTestData.GetTestClientEntities();

            var filter = new ClientInternalFilter
            {
                FirstName = "QWEQWEQWE"
            };
        
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                foreach (var client in clientEntities)
                {
                    await context.Clients.AddAsync(client);
                }
                await context.SaveChangesAsync();
            }

            List<Client> result;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                result = await clientRepository.SearchClients(filter, CancellationToken.None);
            }
            
            // Assert
            Assert.True(result.Count == 0);
        }
        
        [Fact]
        public void SearchClients_WithCanceledToken_ReturnsCanceledTask()
        {
            // Arrange
            var canceledToken = new CancellationToken(canceled: true);
        
            var filter = new ClientInternalFilter
            {
                FirstName = "Данил"
            };
        
            Task task;
            
            using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                task = clientRepository.SearchClients(filter, canceledToken);
            }
        
            // Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task AddClient_WhenSuccessful_AddedClientInDatabase()
        {
            //Arrange
            var client = new Client
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateTime(2002, 06, 01),
                Salary = 50000
            };

            long clientId;

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                clientId = await clientRepository.AddClient(client, CancellationToken.None);
            }

            // Assert
            Assert.Equal(client.Id, clientId);
        }
        
        [Fact]
        public async Task AddClient_WhenNotSuccessful_ThrowsInvalidOperationException()
        {
            //Arrange
            var client = new Client
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateTime(2002, 06, 01),
                Salary = 50000
            };
            
            var clientEntity = new ClientEntity()
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateOnly(2002, 06, 01),
                Salary = 50000
            };

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                context.ChangeTracker.TrackGraph(clientEntity, e => e.Entry.State = EntityState.Added);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    clientRepository.AddClient(client, CancellationToken.None));
            }
        }
        
        [Fact]
        public void AddClient_WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);
            
            var client = new Client
            {
                Id = 1,
                FirstName = "Дима",
                LastName = "Матвеев",
                BirthDate = new DateTime(2002, 06, 01),
                Salary = 50000
            };
            
            Task task;
            
            using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                task = clientRepository.AddClient(client, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task UpdateClient_WhenSuccessful_UpdatedClientInDatabase()
        {
            //Arrange
            var clientEntities = _clientEntityTestData.GetTestClientEntities();
            var clientEntity = clientEntities[0];

            var updatedClient = new Client()
            {
                Id = 1,
                FirstName = "Данил",
                LastName = "Петров", 
                BirthDate = new DateTime(2000, 01, 04),
                Salary = 66666
            };
        
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                await context.Clients.AddAsync(clientEntity);
                await context.SaveChangesAsync();
            }
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                await clientRepository.UpdateClient(updatedClient, CancellationToken.None);
            }
            
            // Assert
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                var result = await context.Clients.FindAsync(updatedClient.Id);

                Assert.NotNull(result);
                Assert.Equal(updatedClient.FirstName, result.FirstName);
                Assert.Equal(updatedClient.LastName, result.LastName);
                Assert.Equal(updatedClient.MiddleName, result.MiddleName);
                Assert.Equal(updatedClient.BirthDate.Date, new DateTime(result.BirthDate.Year, result.BirthDate.Month, result.BirthDate.Day));
                Assert.Equal(updatedClient.Salary, result.Salary);
            }
        }
        
        [Fact]
        public async Task UpdateClient_WhenNotSuccessful_ThrowsException()
        {
            //Arrange
            var clientEntities = _clientEntityTestData.GetTestClientEntities();
            var clientEntity = clientEntities[0];

            var updatedClient = new Client()
            {
                Id = 1,
                FirstName = "Данил",
                LastName = "Петров", 
                BirthDate = new DateTime(2000, 01, 04),
                Salary = 66666
            };

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                context.ChangeTracker.TrackGraph(clientEntity, e => e.Entry.State = EntityState.Added);

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() =>
                    clientRepository.UpdateClient(updatedClient, CancellationToken.None));
            }
        }
        
        [Fact]
        public void UpdateClient_WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);

            var updatedClient = new Client()
            {
                Id = 1,
                FirstName = "Данил",
                LastName = "Петров", 
                BirthDate = new DateTime(2000, 01, 04),
                Salary = 66666
            };

            Task task;

            using (var context = new LoansDbContext(_dbContextOptions))
            {
                IClientRepository clientRepository = new ClientRepository(context);
                
                //Act
                task = clientRepository.UpdateClient(updatedClient, canceledToken);
            }

            //Assert
            Assert.True(task.IsCanceled);
        }
    }
}