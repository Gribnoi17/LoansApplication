using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.Contracts.Clients.Handlers
{
    /// <summary>
    /// Обработчик для поиска клиента.
    /// </summary>
    public interface ISearchClientsHandler
    {
        /// <summary>
        /// Обрабатывает запрос по поиску клиента/клиентов по фильтру.
        /// </summary>
        /// <param name="internalFilter">Параметры фильтра.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Данные клиента/клиентов.</returns>
        Task<List<Client>> Handle(ClientInternalFilter internalFilter, CancellationToken token);
    }
}
