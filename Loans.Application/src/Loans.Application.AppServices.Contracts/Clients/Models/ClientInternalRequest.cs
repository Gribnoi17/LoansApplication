namespace Loans.Application.AppServices.Contracts.Clients.Models
{
    /// <summary>
    /// Модель данных клиента для входных запросов.
    /// </summary>
    public class ClientInternalRequest
    {
        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string LastName { get; set; }
        
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