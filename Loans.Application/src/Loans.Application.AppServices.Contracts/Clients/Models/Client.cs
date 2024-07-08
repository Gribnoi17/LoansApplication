namespace Loans.Application.AppServices.Contracts.Clients.Models
{
    /// <summary>
    /// Главная модель данных клиента.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public long Id { get; set; }
        
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
        
        /// <summary>
        /// Зарплата клиента.
        /// </summary>
        public decimal Salary { get; set; }
    }
}
