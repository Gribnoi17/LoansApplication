using System.Text.RegularExpressions;
using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Clients.Validators;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;

namespace Loans.Application.AppServices.Clients.Validators
{
    /// <inheritdoc />
    internal class ClientValidator : IClientValidator
    {
        private const int _minimumAgeOfClient = 18;

        public void Validate(ClientInternalRequest request)
        {
            var validationErrors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                validationErrors.Add("Не указано имя клиента.");
            }
            
            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                validationErrors.Add("Не указана фамилия клиента.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.MiddleName))
            {
                if (!Regex.IsMatch(request.MiddleName, "^[A-Za-zА-Яа-я ]+$"))
                {
                    validationErrors.Add("Отчество содержит недопустимые символы.");
                }
            }
            
            if (request.BirthDate == DateTime.MinValue)
            {
                validationErrors.Add("Не указана дата рождения.");
            }
            else
            {
                if (!IsAdult(request.BirthDate, DateTime.Now))
                {
                    validationErrors.Add("Клиент моложе 18 лет.");
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }
        }

        /// <summary>
        /// Проверяет, является ли лицо взрослым на основе его даты рождения и текущей даты.
        /// </summary>
        /// <param name="birthDate">Дата рождения для проверки.</param>
        /// <param name="currentDate">Текущая дата для определения взрослости.</param>
        /// <returns>True, если лицо считается взрослым; в противном случае - false.</returns>
        internal static bool IsAdult(DateTime? birthDate, DateTime currentDate)
        {
            if (!birthDate.HasValue)
            {
                return false;
            }
            
            var age = currentDate.Year - birthDate.Value.Year;
        
            if (birthDate.Value.AddYears(age) > currentDate)
            {
                age--;
            }
        
            return age >= _minimumAgeOfClient;
        }
    }
}
