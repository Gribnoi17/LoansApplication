using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Loans.Repository;
using Loans.Application.DataAccess.Clients.Repository;
using Loans.Application.DataAccess.Data;
using Loans.Application.DataAccess.Loans.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Loans.Application.DataAccess.Infrastructure.DependencyConfiguration
{
    /// <summary>
    /// Класс, предоставляющий методы для регистрации зависимостей в контейнере DI.
    /// </summary>
    public static class DependencyRegistration
    {
        private const string _databaseConnectionSection = "DatabaseConnectionString";
        private const string _defaultConnection = "DefaultConnection";
        
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            
            services.AddScoped<ILoanContractRepository, LoanContractRepository>();

            return services;
        }

        public static IServiceCollection AddLoansDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConnectionString = configuration.GetSection(_databaseConnectionSection)[_defaultConnection];
            
            services.AddDbContext<LoansDbContext>(
                options => options.UseNpgsql(databaseConnectionString));
        
            return services;
        }
    }
}
