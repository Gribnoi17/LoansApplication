namespace Loans.Application.Host.Infrastructure.Middleware
{
    /// <summary>
    /// Middleware для добавления HTTP-заголовка с именем сервиса к ответу.
    /// </summary>
    public class HeaderMiddleware : IMiddleware
    {
        private readonly string _serviceName;

        /// <summary>
        /// Инициализирует новый экземпляр класса HeaderMiddleware с указанием имени сервиса.
        /// </summary>
        /// <param name="serviceName">Имя сервиса, которое будет добавлено в HTTP-заголовок.</param>
        public HeaderMiddleware(string serviceName)
        {
            _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("X-SERVICE-NAME", _serviceName);

            await next(context);
        }
    }
}
