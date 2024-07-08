namespace Loans.Application.Api.Contracts.Clients.Requests
{
    /// <summary>
    /// Модель запроса для обновления информации о клиенте.
    /// </summary>
    public class ClientUpdateRequest
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Заработная плата клиента.
        /// </summary>
        public decimal? Salary { get; set; }
    }
}