namespace Loans.Application.Api.Contracts.Clients.Requests
{
    /// <summary>
    /// Модель запроса для создания нового клиента.
    /// </summary>
    public record ClientRequest
    {
        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество клиента (может быть null).
        /// </summary>
        public string? MiddleName { get; set; }
        
        /// <summary>
        /// Дата рождения клиента.
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
