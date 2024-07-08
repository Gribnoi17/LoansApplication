using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Microsoft.Extensions.Logging;

namespace Loans.Application.AppServices.Clients.Handlers
{
    /// <inheritdoc />
    internal class SearchClientsHandler : ISearchClientsHandler
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<SearchClientsHandler> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса SearchClientHandler.
        /// </summary>
        /// <param name="clientRepository">Репозиторий клиентов</param>
        /// <param name="logger">Logger сообщений.</param>
        public SearchClientsHandler(IClientRepository clientRepository, ILogger<SearchClientsHandler> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<List<Client>> Handle(ClientInternalFilter internalFilter, CancellationToken token)
        {
            _logger.LogInformation( "Поиск клиента/ов начался!");
            var clients = await _clientRepository.SearchClients(internalFilter, token);

            _logger.LogInformation( "Поиск клиента/ов успешно закончился!");
            return clients.ToList();
            
        }
    }
}
