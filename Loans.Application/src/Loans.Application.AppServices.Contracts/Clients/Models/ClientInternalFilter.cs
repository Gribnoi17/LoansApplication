namespace Loans.Application.AppServices.Contracts.Clients.Models
{
    /// <summary>
    /// Внутренний фильтр для поиска клиентов.
    /// </summary>
    public class ClientInternalFilter
    {
        /// <summary>
        /// Фильтр по имени клиента.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фильтр по фамилии клиента.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Фильтр по отчеству клиента.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Фильтр по дате рождения клиента.
        /// </summary>
        public DateTime? BirthDate { get; set; }
    }
}