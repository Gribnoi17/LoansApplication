using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.Contracts.Clients.Handlers
{
    /// <summary>
    /// Обработчик для регистрации нового клиента.
    /// </summary>
    public interface ICreateClientHandler
    {
        /// <summary>
        /// Обрабатывает запрос на регистрацию клиента.
        /// </summary>
        /// <param name="clientInternalRequest">Данные клиента.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Модель клиента.</returns>
        Task<Client> Handle(ClientInternalRequest clientInternalRequest, CancellationToken token);
    }
}
