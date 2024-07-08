using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Loans.Application.Host.Infrastructure.CustomFilter
{
    /// <summary>
    /// Фильтр исключений для обработки исключительных ситуаций в контроллерах.
    /// </summary>
    public class ExceptionFilter : IActionFilter
    {
        private ILogger<ExceptionFilter> _logger;
        
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is ValidationException validationException)
                {
                    context.Result = new BadRequestObjectResult(validationException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is ArgumentNullException argumentNullException)
                {
                    context.Result = new BadRequestObjectResult(argumentNullException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is InvalidOperationException operationException)
                {
                    context.Result = new BadRequestObjectResult(operationException.Message);
                    context.ExceptionHandled = true; 
                }
                else if (context.Exception is Exception ex)
                {
                    context.Result = new NotFoundObjectResult(ex.Message);
                    context.ExceptionHandled = true;
                }
                
                _logger.LogError(context.Exception.Message);
            }
        }
    }
}