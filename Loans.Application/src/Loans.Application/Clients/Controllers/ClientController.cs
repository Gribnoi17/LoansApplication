using Loans.Application.Api.Contracts.Clients.Controllers;
using Loans.Application.Api.Contracts.Clients.Requests;
using Loans.Application.Api.Contracts.Clients.Responses;
using Loans.Application.Api.Contracts.Loans.Responses;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.Host.Infrastructure.CustomFilter;
using Loans.Application.Host.Infrastructure.MapService;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Application.Host.Clients.Controllers
{
    /// <inheritdoc cref="IClientController" />
    [ApiController]
    [Route("client")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ClientController : Controller, IClientController
    {
        private readonly ICreateClientHandler _createClientHandler;
        private readonly ISearchClientsHandler _searchClientsHandler;
        private readonly IUpdateClientHandler _updateClientHandler;
        private readonly IGetLoanContractsByClientIdHandler _getLoanContractsByClientIdHandler;
        private readonly MappingService _mappingService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера ClientController.
        /// </summary>
        /// <param name="createClientHandler">Обработчик создания клиента.</param>
        /// <param name="searchClientsHandler">Обработчик поиска клиента.</param>
        /// <param name="updateClientHandler">Обработчик обновление данных клиента.</param>
        /// <param name="getLoanContractsByClientIdController">Обработчик получения кредитного договора по идентификатору клиента.</param>
        public ClientController(
            ICreateClientHandler createClientHandler,
            ISearchClientsHandler searchClientsHandler,
            IUpdateClientHandler updateClientHandler,
            IGetLoanContractsByClientIdHandler getLoanContractsByClientIdController)
        {
            _createClientHandler = createClientHandler;
            _searchClientsHandler = searchClientsHandler;
            _updateClientHandler = updateClientHandler;
            _getLoanContractsByClientIdHandler = getLoanContractsByClientIdController;
            _mappingService = new MappingService();
        }

        [HttpPost]
        [Route("create")]
        public async Task<ClientResponse> CreateClient(ClientRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Данные не введены!");
            }

            var clientInternalRequest = _mappingService.MapToClientInternalRequest(request);
            
            var client = await _createClientHandler.Handle(clientInternalRequest, token);

            var clientResponse = _mappingService.MapToClientResponse(client);

            return clientResponse;
        }
        
        [HttpGet]
        [Route("{clientId:long:min(1)}/loans")]
        public async Task<LoanContractResponse[]> GetLoanContractsByClientId(long clientId, CancellationToken token)
        {
            var loanContracts =  await _getLoanContractsByClientIdHandler.Handle(clientId, token);
            
            var loanContractResponses = _mappingService.MapToLoanContractResponses(loanContracts);

            return loanContractResponses.ToArray();
        }

        [HttpGet]
        public async Task<ClientResponse[]> FindClients([FromQuery] ClientFilterRequest filterRequest, CancellationToken token)
        {
            var filter = _mappingService.MapToClientInternalFilter(filterRequest);

            var clients = await _searchClientsHandler.Handle(filter, token);
            
            var clientsResponses = _mappingService.MapToClientResponses(clients);

            return clientsResponses.ToArray();
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateClient(ClientUpdateRequest request, CancellationToken token)
        {
            var updateInternalRequest = _mappingService.MapToClientUpdateInternalRequest(request);

            await _updateClientHandler.Handle(updateInternalRequest, token);
        }
    }
}
