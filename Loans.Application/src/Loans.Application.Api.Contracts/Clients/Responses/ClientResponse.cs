namespace Loans.Application.Api.Contracts.Clients.Responses
{
    /// <summary>
    /// Модель ответа для представления информации о клиенте.
    /// </summary>
    public class ClientResponse
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>   
        public long Id { get; set; }
        
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

        /// <summary>
        /// Зарплата клиента.
        /// </summary>
        public decimal Salary { get; set; }
    }
}