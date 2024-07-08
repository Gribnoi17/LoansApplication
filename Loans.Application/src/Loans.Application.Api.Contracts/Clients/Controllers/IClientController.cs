using Loans.Application.Api.Contracts.Clients.Requests;
using Loans.Application.Api.Contracts.Clients.Responses;
using Loans.Application.Api.Contracts.Loans.Responses;

namespace Loans.Application.Api.Contracts.Clients.Controllers
{
    /// <summary>
    /// Предназначен для работы с клиентами.
    /// </summary>
    public interface IClientController
    {
        /// <summary>
        /// Создание нового клиента.
        /// </summary>
        /// <param name="request">Данные клиента для регистрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Модель созданного клиента.</returns>
        Task<ClientResponse> CreateClient(ClientRequest request, CancellationToken token);

        /// <summary>
        /// Поиск клиента по его идентификатору.
        /// </summary>
        /// <param name="filter">Параметр фильтрации</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Информация о клиенте.</returns>
        Task<ClientResponse[]> FindClients(ClientFilterRequest filter, CancellationToken token);
        
        /// <summary>
        /// Обновляет информацию о клиенте.
        /// </summary>
        /// <param name="request">Запрос на обновление клиентских данных.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Информация о клиенте.</returns>
        Task UpdateClient(ClientUpdateRequest request, CancellationToken token);
        
        /// <summary>
        /// Получение кредитного договора по идентификатору клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Информация о кредитных договорах.</returns>
        Task<LoanContractResponse[]> GetLoanContractsByClientId(long clientId, CancellationToken token);
    }
}