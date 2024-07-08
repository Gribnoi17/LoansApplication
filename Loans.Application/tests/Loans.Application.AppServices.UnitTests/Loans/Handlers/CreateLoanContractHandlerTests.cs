using DCS.DecisionMakerService.Client.Kafka.Events;
using Loans.Application.AppServices.Clients.Handlers;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Infrastructure.Kafka.Producers;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Contracts.Loans.Validators;
using Loans.Application.AppServices.Loans.Handlers;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.UnitTests.Loans.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Loans.Application.AppServices.UnitTests.Loans.Handlers
{
    public class CreateLoanContractHandlerTests
    {
        private readonly ILoanValidator _loanValidator;
        private readonly IClientRepository _clientRepository;
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ICalculateDecisionProducer _calculateDecisionProducer;
        private readonly ICreateLoanContractHandler _createLoanContractHandler;
        private readonly ILogger<CreateLoanContractHandler> _logger;

        public CreateLoanContractHandlerTests()
        {
            _loanValidator = Substitute.For<ILoanValidator>();
            
            _clientRepository = Substitute.For<IClientRepository>();

            _loanContractRepository = Substitute.For<ILoanContractRepository>();

            _calculateDecisionProducer = Substitute.For<ICalculateDecisionProducer>();
            
            _logger = Substitute.For<ILogger<CreateLoanContractHandler>>();
            
            _createLoanContractHandler = new CreateLoanContractHandler(_loanValidator, _clientRepository, _calculateDecisionProducer, _loanContractRepository, _logger);
        }

        [Theory]
        [ClassData(typeof(ValidLoanContractInternalRequestTestData))]
        public async Task Handle_WithValidData_SendsCorrectDataToCalculateDecisionProducer(LoanContractInternalRequest loanRequest)
        {
            //A
            var client = new Client { Id = 1, FirstName = "Данил", LastName = "Китов", BirthDate = new DateTime(2000, 01, 04) };
            long loanContractId = 1;
            
            _clientRepository.GetClientById(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Returns(client);

            _loanContractRepository.AddLoanContract(Arg.Any<LoanContract>(), Arg.Any<CancellationToken>())
                .Returns(loanContractId);

            // Act
            var result = await _createLoanContractHandler.Handle(loanRequest, CancellationToken.None);

            // Assert
            Assert.Equal(client.Salary, loanRequest.Salary);
            
            _loanValidator.Received(1).Validate(loanRequest);
            
            await _loanContractRepository.Received(1).AddLoanContract(Arg.Is<LoanContract>(
                lc => lc.ClientId == loanRequest.ClientId &&
                      lc.LoanTermMonth == loanRequest.LoanTermMonth &&
                      lc.Amount == loanRequest.Amount 
            ), Arg.Any<CancellationToken>());
            
            
            await _calculateDecisionProducer.Received(1).SendCalculateDecisionEvent(Arg.Is<CalculateDecisionEvent>(
                decisionEvent => decisionEvent.ClientId == loanRequest.ClientId &&
                                 decisionEvent.BirthDay == client.BirthDate &&
                                 decisionEvent.CreditAmount == loanRequest.Amount &&
                                 decisionEvent.CreditLenMonth == loanRequest.LoanTermMonth &&
                                 decisionEvent.IncomeAmount == client.Salary &&
                                 decisionEvent.ApplicationDate != DateTime.MinValue &&
                                 decisionEvent.ApplicationId == loanContractId));

            Assert.True(result != null);
        }
        
        [Fact]
        public async Task Handle_GetClientByIdFails_ThrowsException()
        {
            _clientRepository.GetClientById(Arg.Any<long>(), Arg.Any<CancellationToken>())
                .Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(
                () => _createLoanContractHandler.Handle(new LoanContractInternalRequest(), CancellationToken.None)
            );
        }
        
        [Fact]
        public async Task Handle_UpdateClientFails_ThrowsException()
        {
            _clientRepository.UpdateClient(Arg.Any<Client>(), Arg.Any<CancellationToken>())
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _createLoanContractHandler.Handle(new LoanContractInternalRequest(), CancellationToken.None)
            );
        }
        
        [Fact]
        public async Task Handle_AddLoanContractFails_ThrowsException()
        {
            _loanContractRepository.AddLoanContract(Arg.Any<LoanContract>(), Arg.Any<CancellationToken>())
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _createLoanContractHandler.Handle(new LoanContractInternalRequest(), CancellationToken.None)
            );
        }
        
        [Fact]
        public async Task Handle_SendCalculateDecisionEventFails_ThrowsException()
        {
            _calculateDecisionProducer.SendCalculateDecisionEvent(Arg.Any<CalculateDecisionEvent>())
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _createLoanContractHandler.Handle(new LoanContractInternalRequest(), CancellationToken.None)
            );
        }
    }
}

