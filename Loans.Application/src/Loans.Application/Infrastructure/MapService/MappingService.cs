using Loans.Application.Api.Contracts.Clients.Requests;
using Loans.Application.Api.Contracts.Clients.Responses;
using Loans.Application.Api.Contracts.Loans.Enum;
using Loans.Application.Api.Contracts.Loans.Requests;
using Loans.Application.Api.Contracts.Loans.Responses;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.Host.Infrastructure.MapService
{
    /// <summary>
    /// Сервис "маппинга" данных.
    /// </summary>
    public class MappingService
    {
        /// <summary>
        /// Преобразует модель данных клиента на объект ответа.
        /// </summary>
        /// <param name="client">Данные клиента.</param>
        /// <returns>Объект ответа с данными о клиента.</returns>
        public ClientResponse MapToClientResponse(Client client)
        {
            var clientResponse = new ClientResponse
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                Salary = client.Salary,
            };

            return clientResponse;
        }

        /// <summary>
        /// Преобразует объект запроса клиента на объект входных данных клиента для использования в бизнес-логике.
        /// </summary>
        /// <param name="request">Объект запроса с данными клиента.</param>
        /// <returns>Объект входных данных клиента.</returns>
        public ClientInternalRequest MapToClientInternalRequest(ClientRequest request)
        {
            var сlientInternalRequest = new ClientInternalRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                BirthDate = request.BirthDate
            };
            
            return сlientInternalRequest;
        }
        
        /// <summary>
        /// Преобразует запрос на обновление клиента во внутренний запрос для использования в бизнес-логики.
        /// </summary>
        /// <param name="updateRequest">Запрос на обновление клиента.</param>
        /// <returns>Внутренний запрос на обновление клиента для использования в бизнес-логике.</returns>
        public ClientUpdateInternalRequest MapToClientUpdateInternalRequest(ClientUpdateRequest updateRequest)
        {
            return new ClientUpdateInternalRequest
            {
                Id = updateRequest.Id,
                FirstName = updateRequest.FirstName,
                LastName = updateRequest.LastName,
                MiddleName = updateRequest.MiddleName,
                Salary = updateRequest.Salary
            };
        }
        
        /// <summary>
        /// Преобразует фильтр запроса клиента во внутренний фильтр для использования в бизнес-логике.
        /// </summary>
        /// <param name="filterRequest">Фильтр запроса клиента.</param>
        /// <returns>Внутренний фильтр для использования в бизнес-логики.</returns>
        public ClientInternalFilter MapToClientInternalFilter(ClientFilterRequest filterRequest)
        {
            return new ClientInternalFilter
            {
                FirstName = filterRequest.FirstName,
                LastName = filterRequest.LastName,
                MiddleName = filterRequest.MiddleName,
                BirthDate = filterRequest.BirthDate
            };
        }
        
        /// <summary>
        /// Преобразует список клиентов в список клиентов для ответа пользователю.
        /// </summary>
        /// <param name="clients">Список клиентов для преобразования.</param>
        /// <returns>Список клиентов для ответа пользователю.</returns>
        public List<ClientResponse> MapToClientResponses(List<Client> clients)
        {
            var clientResponses = new List<ClientResponse>();

            foreach (var client in clients)
            {
                var clientResponse = MapToClientResponse(client);
                clientResponses.Add(clientResponse);
            }

            return clientResponses;
        }
        
        /// <summary>
        /// Преобразует кредитный договор на объект ответа пользователю.
        /// </summary>
        /// <param name="loanContract">Данные кредитного договора.</param>
        /// <returns>Объект ответа пользователю с данными о кредитном договоре.</returns>
        public LoanContractResponse MapToLoanContractResponse(LoanContract loanContract)
        {
            var interestRateMultiplier = 100;
            
            return new LoanContractResponse
            {
                Id = loanContract.Id,
                Amount = loanContract.Amount,
                InterestRate = loanContract.InterestRate * interestRateMultiplier,
                LoanTermMonth = loanContract.LoanTermMonth,
                LoanDate = loanContract.LoanDate,
                RejectionReason = loanContract.RejectionReason,
                Status = MapToLoanContractStatusResponse(loanContract.Status)
            };
        }
        
        /// <summary>
        /// Преобразует кредитные договора на объект ответа пользователю.
        /// </summary>
        /// <param name="loanContracts">Данные о кредитных договорах.</param>
        /// <returns>Объект ответа пользователю с данными о кредитных договорах.</returns>
        public List<LoanContractResponse> MapToLoanContractResponses(List<LoanContract> loanContracts)
        {
            var loanContractResponses = new List<LoanContractResponse>();

            foreach (var loanContract in loanContracts)
            {
                var clientResponse = MapToLoanContractResponse(loanContract);
                loanContractResponses.Add(clientResponse);
            }

            return loanContractResponses;
        }

        /// <summary>
        /// Преобразует объект запроса кредитного договора на объект входных данных кредитного договора для использования в бизнес-логике.
        /// </summary>
        /// <param name="request">Объект запроса с данными кредитного договора.</param>
        /// <returns>Объект входных данных кредитного договора для использования в бизнес-логике.</returns>
        public LoanContractInternalRequest MapToLoanContractInternalRequest(LoanRequest request)
        {
            var loanContractInternalRequest = new LoanContractInternalRequest
            {
                ClientId = request.ClientId,
                Amount = request.Amount,
                LoanTermMonth = request.LoanTermMonth,
                Salary = request.Salary
            };

            return loanContractInternalRequest;
        }

        /// <summary>
        /// Преобразует внутренний статус кредитного договора в соответствующий статус для ответа.
        /// </summary>
        /// <param name="status">Внутренний статус кредитного договора.</param>
        /// <returns>Статус для ответа пользователю, соответствующий статусу кредитного договора.</returns>
        public LoanStatus MapToLoanContractStatusResponse(global::Loans.Application.AppServices.Contracts.Loans.LoanStatus status)
        {
            switch (status)
            {
                case AppServices.Contracts.Loans.LoanStatus.InProgress:
                    return LoanStatus.InProgress;
                case AppServices.Contracts.Loans.LoanStatus.Approved:
                    return LoanStatus.Approved;
                case AppServices.Contracts.Loans.LoanStatus.Denied:
                    return LoanStatus.Denied;
                default:
                    return LoanStatus.Unknown;
            }
        }
    }
}