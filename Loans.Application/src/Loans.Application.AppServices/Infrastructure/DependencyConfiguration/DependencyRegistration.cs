using Loans.Application.AppServices.Clients.Handlers;
using Loans.Application.AppServices.Clients.Validators;
using Loans.Application.AppServices.Loans.Handlers;
using Loans.Application.AppServices.Loans.Validators;
using Loans.Application.AppServices.Contracts.Clients.Handlers;
using Loans.Application.AppServices.Contracts.Clients.Validators;
using Loans.Application.AppServices.Contracts.Loans.Configuration;
using Loans.Application.AppServices.Contracts.Loans.Handlers;
using Loans.Application.AppServices.Contracts.Loans.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Loans.Application.AppServices.Infrastructure.DependencyConfiguration
{
    /// <summary>
    /// Класс, предоставляющий методы для регистрации зависимостей в контейнере DI.
    /// </summary>
    public static class DependencyRegistration
    {
        /// <summary>
        /// Регистрирует обработчики для операций с клиентами и кредитными договорами.
        /// </summary>
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICreateClientHandler, CreateClientHandler>();

            services.AddScoped<ISearchClientsHandler, SearchClientsHandler>();
            
            services.AddScoped<IUpdateClientHandler, UpdateClientHandler>();

            services.AddScoped<IGetLoanContractsByClientIdHandler, GetLoanContractsByClientIdHandler>();

            services.AddScoped<IGetLoanContractByIdHandler, GetLoanContractByIdHandler>();

            services.AddScoped<ICreateLoanContractHandler, CreateLoanContractHandler>();

            services.AddScoped<IGetLoanContractStatusHandler, GetLoanContractStatusHandler>();
            
            services.AddScoped<IProcessLoanContractDecisionHandler, ProcessLoanContractDecisionHandler>();
            
            return services;
        }
        
        /// <summary>
        /// Регистрирует валидаторы для кредитных и клиентских данных.
        /// </summary>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<ILoanValidator, LoanValidator>();

            services.AddScoped<IClientValidator, ClientValidator>();

            return services;
        }

        /// <summary>
        /// Регистрирует конфигурацию кредита из файловой конфигурации.
        /// </summary>
        public static IServiceCollection AddLoanConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LoanSpecification>(configuration.GetSection("LoanSpecification"));
            
            services.AddSingleton<IOptionsMonitor<LoanSpecification>, OptionsMonitor<LoanSpecification>>();

            return services;
        }
    }
}
