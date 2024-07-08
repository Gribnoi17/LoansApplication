using Loans.Application.AppServices.Contracts.Loans;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.DataAccess.Data;
using Loans.Application.DataAccess.Loans.Models;
using Loans.Application.DataAccess.Loans.Repository;
using Loans.Application.DataAccess.UnitTests.Loans.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace Loans.Application.DataAccess.UnitTests.Loans.Repository
{
    public class LoanContractRepositoryTests
    {
        private readonly LoanContractTestData _loanContractTestData;
        private readonly LoanContractEntityTestData _loanContractEntityTestData;
        private readonly DbContextOptions<LoansDbContext> _dbContextOptions;

        public LoanContractRepositoryTests()
        {
            _loanContractTestData = new LoanContractTestData();
            _loanContractEntityTestData = new LoanContractEntityTestData();
            
            _dbContextOptions = new DbContextOptionsBuilder<LoansDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseForLoanContract")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new LoansDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        
        [Theory]
        [ClassData(typeof(LoanContractTestData))]
        public async Task GetLoanContractById_WithValidId_ReturnsLoanContract(LoanContract loanContract)
        {
            //Arange
            var loanContractEntity = new LoanContractEntity
            {
                Id = loanContract.Id,
                ClientId = loanContract.ClientId,
                Amount = loanContract.Amount,
                ExpectedInterestRate = loanContract.InterestRate,
                LoanDate = new DateOnly(loanContract.LoanDate.Date.Year, loanContract.LoanDate.Date.Month, loanContract.LoanDate.Date.Day),
                LoanTermMonth = loanContract.LoanTermMonth,
                RejectionReason = loanContract.RejectionReason,
                Status = loanContract.Status
            };

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                await context.LoanContracts.AddAsync(loanContractEntity);
                await context.SaveChangesAsync();
            }

            LoanContract result;

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act
                result = await loanContractRepository.GetLoanContractById(loanContract.Id, CancellationToken.None);
            }
            
            //Assert
            Assert.Equal(loanContract.Id, result.Id);
            Assert.Equal(loanContract.ClientId, result.ClientId);
            Assert.Equal(loanContract.Amount, result.Amount);
            Assert.Equal(loanContract.InterestRate, result.InterestRate);
            Assert.Equal(loanContract.LoanDate.Date, result.LoanDate.Date);
        }
        
        [Fact]
        public async Task GetLoanContractById_WithInvalidId_ThrowsInvalidOperationException()
        {
            //Arange
            long id = -1;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    loanContractRepository.GetLoanContractById(id, CancellationToken.None));
            }
        }
        
        [Fact]
        public void GetLoanContractById_WithCanceledToken_ReturnsCanceledTask()
        {
            // Arrange
            long validId = 1;
            var canceledToken = new CancellationToken(canceled: true);

            Task task;
            
            using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);;
                
                //Act
                task = loanContractRepository.GetLoanContractById(validId, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task GetLoanContractsByClientId_WithValidClientId_ReturnsLoanContracts()
        {
            //Arange
            var loanContracts = _loanContractTestData.GetLoanContracts();
            var loanContractEntities = _loanContractEntityTestData.GetTestLoanContractEntities();
            var expectedLoanContracts = new List<LoanContract> {loanContracts[1], loanContracts[2] };
            var clientId = loanContracts[1].ClientId;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                foreach (var loanContractEntity in loanContractEntities)
                {
                    await context.LoanContracts.AddAsync(loanContractEntity);
                }
                await context.SaveChangesAsync();
            }
            
            List<LoanContract> result;

            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act
                result = await loanContractRepository.GetLoanContractsByClientId(clientId, CancellationToken.None);
            }
            
            //Assert
            Assert.Equal(expectedLoanContracts.Count, result.Count);
            
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedLoanContracts[i].Id, result[i].Id);
                Assert.Equal(expectedLoanContracts[i].ClientId, result[i].ClientId);
                Assert.Equal(expectedLoanContracts[i].Amount, result[i].Amount);
                Assert.Equal(expectedLoanContracts[i].InterestRate, result[i].InterestRate);
                Assert.Equal(expectedLoanContracts[i].LoanDate.Date, result[i].LoanDate.Date);
                Assert.Equal(expectedLoanContracts[i].LoanTermMonth, result[i].LoanTermMonth);
                Assert.Equal(expectedLoanContracts[i].Status, result[i].Status);
                Assert.Equal(expectedLoanContracts[i].RejectionReason, result[i].RejectionReason);
            }
        }
        
        [Fact]
        public async Task GetLoanContractsByClientId_WithInvalidClientId_ThrowsInvalidOperationException()
        {
            //Arange
            long clientId = -1;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    loanContractRepository.GetLoanContractsByClientId(clientId, CancellationToken.None));
            }
        }
        
        [Fact]
        public void GetLoanContractsByClientId_WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            long validId = 1;
            var canceledToken = new CancellationToken(canceled: true);

            Task task;

            using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);

                //Act
                task = loanContractRepository.GetLoanContractsByClientId(validId, canceledToken);
            }

            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task AddLoanContract_WhenSuccessful_AddedLoanContractInDatabase()
        {
            //Arrange
            var loanContract = new LoanContract
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                InterestRate = 10,
                LoanTermMonth = 30,
                LoanDate = new DateTime(2020, 04, 13),
                Status = LoanStatus.Approved
            };
            
            long resultId;
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act
                resultId = await loanContractRepository.AddLoanContract(loanContract, CancellationToken.None);
            }
            
            //Assert
            Assert.Equal(loanContract.Id, resultId);
        }
        
        [Fact]
        public async Task AddLoanContract_WhenNotSuccessful_ThrowsInvalidOperationException()
        {
            //Arrange
            var loanContractEntities = _loanContractEntityTestData.GetTestLoanContractEntities();
            
            var loanContract = new LoanContract
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                InterestRate = 10,
                LoanTermMonth = 30,
                LoanDate = new DateTime(2020, 04, 13),
                Status = LoanStatus.Approved
            };
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                context.ChangeTracker.TrackGraph(loanContractEntities[0], e => e.Entry.State = EntityState.Added);
                
                //Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    loanContractRepository.AddLoanContract(loanContract, CancellationToken.None));
            }
        }
        
        [Theory]
        [ClassData(typeof(LoanContractTestData))]
        public void AddLoanContract_WithCanceledToken_ReturnsCanceledTask(LoanContract loanContract)
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);

            Task task;

            using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);

                //Act
                task = loanContractRepository.AddLoanContract(loanContract, canceledToken);
            }

            //Assert
            Assert.True(task.IsCanceled);
        }

        [Fact]
        public async Task UpdateLoanContract_WhenSuccessful_UpdatedLoanContractInDatabase()
        {
            //Arrange
            var loanContractEntities = _loanContractEntityTestData.GetTestLoanContractEntities();
            var loanContractEntity = loanContractEntities[0];

            var updatedLoanContract = new LoanContract
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                InterestRate = 22,
                LoanTermMonth = 30,
                LoanDate = new DateTime(2020, 04, 13),
                Status = LoanStatus.Denied,
                RejectionReason = "Отмена"
            };
        
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                await context.LoanContracts.AddAsync(loanContractEntity);
                await context.SaveChangesAsync();
            }
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                //Act
                await loanContractRepository.UpdateLoanContract(updatedLoanContract, CancellationToken.None);
            }
            
            // Assert
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                var result = await context.LoanContracts.FindAsync(updatedLoanContract.Id);

                Assert.NotNull(result);
                Assert.Equal(updatedLoanContract.Id, result.Id);
                Assert.Equal(updatedLoanContract.ClientId, result.ClientId);
                Assert.Equal(updatedLoanContract.Amount, result.Amount);
                Assert.Equal(updatedLoanContract.InterestRate, result.ExpectedInterestRate);
                Assert.Equal(updatedLoanContract.LoanDate.Date, new DateTime(result.LoanDate.Year, result.LoanDate.Month, result.LoanDate.Day));
                Assert.Equal(updatedLoanContract.LoanTermMonth, result.LoanTermMonth);
                Assert.Equal(updatedLoanContract.Status, result.Status);
                Assert.Equal(updatedLoanContract.RejectionReason, result.RejectionReason);
            }
        }
        
        [Fact]
        public async Task UpdateLoanContract_WhenNotSuccessful_ThrowsException()
        {
            //Arrange
            var loanContractEntities = _loanContractEntityTestData.GetTestLoanContractEntities();
            var loanContractEntity = loanContractEntities[0];
            
            var loanContract = new LoanContract
            {
                Id = 1,
                ClientId = 1,
                Amount = 80000,
                InterestRate = 10,
                LoanTermMonth = 30,
                LoanDate = new DateTime(2020, 04, 13),
                Status = LoanStatus.Approved
            };
            
            await using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);
                
                context.ChangeTracker.TrackGraph(loanContractEntity, e => e.Entry.State = EntityState.Added);
                
                //Act & Assert
                await Assert.ThrowsAsync<Exception>(() =>
                    loanContractRepository.UpdateLoanContract(loanContract, CancellationToken.None));
            }
        }

        [Theory]
        [ClassData(typeof(LoanContractTestData))]
        public void UpdateLoanContract_WithCanceledToken_ReturnsCanceledTask(LoanContract loanContract)
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);

            Task task;

            using (var context = new LoansDbContext(_dbContextOptions))
            {
                ILoanContractRepository loanContractRepository = new LoanContractRepository(context);

                //Act
                task = loanContractRepository.UpdateLoanContract(loanContract, canceledToken);
            }

            //Assert
            Assert.True(task.IsCanceled);
        }
    }
}