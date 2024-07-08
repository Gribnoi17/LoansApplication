namespace Loans.Application.AppServices.Contracts.Infrastructure.Validators
{
    /// <summary>
    /// Предоставляет валидатор для проверки входных моделей.
    /// </summary>
    /// <typeparam name="T">Тип модели, подлежащей валидации.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Проверяет переданную модель на соответствие определенным правилам валидации.
        /// </summary>
        /// <param name="request">Модель, подлежащая валидации.</param>
        void Validate(T request);
    }
}
