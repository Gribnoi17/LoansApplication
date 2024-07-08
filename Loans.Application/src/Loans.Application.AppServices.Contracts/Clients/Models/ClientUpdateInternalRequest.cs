namespace Loans.Application.AppServices.Contracts.Clients.Models
{
    /// <summary>
    /// Внутренняя модель для обновления данных клиента.
    /// </summary>
    public class ClientUpdateInternalRequest
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string? LastName { get; set; }

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