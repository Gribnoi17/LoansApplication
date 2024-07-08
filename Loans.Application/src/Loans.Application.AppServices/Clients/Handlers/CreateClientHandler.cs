using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Clients.Validators;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Clients.Handlers
{
    /// <inheritdoc />
    internal class CreateClientHandler : ICreateClientHandler
    {
        private readonly IClientValidator _clientValidator;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<CreateClientHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса CreateClientHandler.
        /// </summary>
        /// <param name="clientValidator">Валидатор клиента для проверки входных данных.</param>
        /// <param name="clientRepository">Репозиторий клиентов.</param>
        /// <param name="logger">Logger сообщений.</param>
        public CreateClientHandler(IClientValidator clientValidator, IClientRepository clientRepository, ILogger<CreateClientHandler> logger)
        {
            _clientValidator = clientValidator;
            _clientRepository = clientRepository;
            _logger = logger;

        }
        public async Task<Client> Handle(ClientInternalRequest clientInternalRequest, CancellationToken token)
        {
            _logger.LogInformation("Началось создание клиента!");
            _clientValidator.Validate(clientInternalRequest);

            var client = new Client
            {
                FirstName = clientInternalRequest.FirstName,
                LastName = clientInternalRequest.LastName,
                MiddleName = clientInternalRequest.MiddleName,
                BirthDate = clientInternalRequest.BirthDate
            };
            
            var clientId = await _clientRepository.AddClient(client, token);
            _logger.LogTrace("Клиент с Id: {clientId} добавлен в базу данных!", clientId);
            client.Id = clientId;
            
            _logger.LogInformation("Клиент с Id: {clientId} успешно создан!", clientId);
            return client;
        }
    }
}
