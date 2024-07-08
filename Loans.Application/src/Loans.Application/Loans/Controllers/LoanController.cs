using Loans.Application.Api.Contracts.Loans.Controllers;
using Loans.Application.Api.Contracts.Loans.Requests;
using Loans.Application.Api.Contracts.Loans.Responses;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.Host.Infrastructure.CustomFilter;
using Loans.Application.Host.Infrastructure.MapService;
using Microsoft.AspNetCore.Mvc;

namespace Loans.Application.Host.Loans.Controllers
{
    /// <inheritdoc cref="ILoanController" />
    [ApiController]
    [Route("loans")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class LoanController : Controller, ILoanController
    {
        private readonly MappingService _mappingService;
        
        private readonly ICreateLoanContractHandler _createLoanContractHandler;
        private readonly IGetLoanContractByIdHandler _getLoanContractByIdHandler;
        private readonly IGetLoanContractStatusHandler _getLoanContractStatusHandler;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера LoanController.
        /// </summary>
        /// <param name="createLoanContractHandler">Обработчик создания кредитных договоров.</param>
        /// <param name="getLoanContractByIdController">Обработчик получения кредитного договора по идентификатору.</param>
        /// <param name="getLoanContractStatusHandler">Обработчик получения статуса кредитного договора.</param>
        public LoanController(ICreateLoanContractHandler createLoanContractHandler,
            IGetLoanContractByIdHandler getLoanContractByIdController,
            IGetLoanContractStatusHandler getLoanContractStatusHandler)
        {
            _createLoanContractHandler = createLoanContractHandler;
            _getLoanContractByIdHandler = getLoanContractByIdController;
            _getLoanContractStatusHandler = getLoanContractStatusHandler;
            _mappingService = new MappingService();
        }

        [HttpPost]
        public async Task<LoanContractResponse> CreateLoanContract(LoanRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Данные не введены!");
            }

            var loanContractInternalRequest = _mappingService.MapToLoanContractInternalRequest(request);
            
            var loanContract = await _createLoanContractHandler.Handle(loanContractInternalRequest, token);

            var loanContractResponse = _mappingService.MapToLoanContractResponse(loanContract);

            return loanContractResponse;
        }

        [HttpGet]
        [Route("{id:long:min(1)}")]
        public async Task<LoanContractResponse> GetLoanContractById(long id, CancellationToken token)
        {
            var loanContract = await _getLoanContractByIdHandler.Handle(id, token);

            var loanContractResponse = _mappingService.MapToLoanContractResponse(loanContract);

            return loanContractResponse;
        }

        [HttpGet]
        [Route("{id:long:min(1)}/status")]
        public async Task<string> CheckStatus(long id, CancellationToken token)
        {
            var loanStatus = await _getLoanContractStatusHandler.Handle(id, token);

            return loanStatus.ToString();
        }
    }
}
