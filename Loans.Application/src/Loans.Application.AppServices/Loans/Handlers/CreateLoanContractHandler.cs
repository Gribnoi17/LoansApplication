using DCS.DecisionMakerService.Client.Kafka.Events;
using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Infrastructure.Kafka.Producers;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Contracts.Loans.Validators;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Loans.Handlers
{
    /// <inheritdoc />
    internal class CreateLoanContractHandler : ICreateLoanContractHandler
    {
        private readonly ILoanValidator _loanValidator;
        private readonly ICalculateDecisionProducer _calculateDecisionProducer;
        private readonly IClientRepository _clientRepository;
        private readonly ILoanContractRepository _loanContractRepository;
        private readonly ILogger<CreateLoanContractHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса CreateLoanContractHandler.
        /// </summary>
        /// <param name="loanValidator">Валидатор для проверки данных кредитного договора.</param>
        /// <param name="clientRepository">Репозиторий клиентов.</param>
        /// <param name="calculateDecisionProducer">Продюсер для отправки заявки в сервис приянтия решений.</param>
        /// <param name="loanContractRepository">Репозиторий кредитных договоров.</param>
        /// <param name="logger">Logger сообщений.</param>
        public CreateLoanContractHandler(ILoanValidator loanValidator,
            IClientRepository clientRepository,
            ICalculateDecisionProducer calculateDecisionProducer,
            ILoanContractRepository loanContractRepository, ILogger<CreateLoanContractHandler> logger)
        {
            _loanValidator = loanValidator;
            _clientRepository = clientRepository;
            _loanContractRepository = loanContractRepository;
            _logger = logger;
            _calculateDecisionProducer = calculateDecisionProducer;
        }

        public async Task<LoanContract> Handle(LoanContractInternalRequest request, CancellationToken token)
        {
            _logger.LogInformation( "Началось создание кредитной заявки!");
            
            _logger.LogTrace("Получение клиента с Id: {ClientId}!", request.ClientId);
            var client = await _clientRepository.GetClientById(request.ClientId, token);
            client.Salary = request.Salary;
            
            _logger.LogTrace("Обновление данных о зарплате клиента с Id: {ClientId}!", request.ClientId);
            await _clientRepository.UpdateClient(client, token);

            _loanValidator.Validate(request);

            var loanContract = new LoanContract
            { 
                ClientId = request.ClientId,
                Amount = request.Amount,
                LoanTermMonth = request.LoanTermMonth
            };
            
            var loanContractId = await _loanContractRepository.AddLoanContract(loanContract, token);
            _logger.LogInformation( "Кредитная заявка с Id: {loanContractId} успешно создана!", loanContractId);

            loanContract.Id = loanContractId;

            var decisionEvent = new CalculateDecisionEvent()
            {
                ClientId = request.ClientId,
                BirthDay = client.BirthDate,
                CreditAmount = request.Amount,
                CreditLenMonth = request.LoanTermMonth,
                IncomeAmount = client.Salary,
                ApplicationDate = loanContract.LoanDate,
                ApplicationId = loanContractId
            };

            await _calculateDecisionProducer.SendCalculateDecisionEvent(decisionEvent);

            return loanContract;
        }
    }
}
