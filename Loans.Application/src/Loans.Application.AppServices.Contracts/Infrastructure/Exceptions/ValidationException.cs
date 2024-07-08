namespace Loans.Application.AppServices.Contracts.Infrastructure.Exceptions
{
    /// <summary>
    /// Исключение, которое возникает при валидации данных и содержит список ошибок валидации.
    /// </summary>
    public class ValidationException : Exception
    {
        private string _validationErrors = "";

        /// <summary>
        /// Инициализирует новый экземпляр класса `ValidationException` с указанным списком ошибок валидации.
        /// </summary>
        /// <param name="validationErrors">Список ошибок валидации.</param>
        public ValidationException(List<string> validationErrors)
        {
            foreach (var error in validationErrors)
            {
                _validationErrors += error + '\n';
            }
        }

        /// <summary>
        /// Сообщение об исключении, содержащее список ошибок валидации.
        /// </summary>
        public override string Message => $"{_validationErrors}";
    }
}
