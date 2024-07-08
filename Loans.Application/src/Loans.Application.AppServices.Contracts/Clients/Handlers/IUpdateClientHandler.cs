using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.Contracts.Clients.Handlers
{
    /// <summary>
    /// Обработчик для обновления информации о клиенте.
    /// </summary>
    public interface IUpdateClientHandler
    {
        /// <summary>
        /// Обновляет данные клиента на основе внутреннего запроса.
        /// </summary>
        /// <param name="request">Внутренний запрос на обновление клиента.</param>
        /// <param name="token">Токен для отмены операции.</param>
        Task Handle(ClientUpdateInternalRequest request, CancellationToken token);
    }
}