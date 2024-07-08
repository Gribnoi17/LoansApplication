using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.Clients.Repository
{
    /// <summary>
    /// Репозиторий для работы с клиентами.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Получает клиента по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Клиент с указанным идентификатором.</returns>
        Task<Client> GetClientById(long id, CancellationToken token);

        /// <summary>
        /// Поиск клиентов с использованием внутреннего фильтра.
        /// </summary>
        /// <param name="internalFilter">Внутренний фильтр для поиска клиентов.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Список клиентов, соответствующих фильтру.</returns>
        Task<List<Client>> SearchClients(ClientInternalFilter internalFilter, CancellationToken token);

        /// <summary>
        /// Добавляет нового клиента в память.
        /// </summary>
        /// <param name="client">Данные нового клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task<long> AddClient(Client client, CancellationToken token);

        /// <summary>
        /// Обновляет информацию о клиенте.
        /// </summary>
        /// <param name="client">Данные клиента для обновления.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateClient(Client client, CancellationToken token);
    }
}