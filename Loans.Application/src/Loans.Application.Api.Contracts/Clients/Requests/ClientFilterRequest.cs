namespace Loans.Application.Api.Contracts.Clients.Requests
{
    /// <summary>
    /// Модель запроса для фильтрации клиентов по определенным критериям.
    /// </summary>
    public class ClientFilterRequest
    {
        /// <summary>
        /// Фильтр по фамилии клиента.
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// Фильтр по имени клиента.
        /// </summary>
        public string? FirstName { get; set; }

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