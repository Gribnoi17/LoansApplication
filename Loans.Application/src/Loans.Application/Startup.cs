using Loans.Application.AppServices.Infrastructure.DependencyConfiguration;
using Loans.Application.DataAccess.Infrastructure.DependencyConfiguration;
using Loans.Application.Host.Infrastructure.Middleware;
using Loans.Application.Host.Infrastructure.Extensions;

namespace Loans.Application.Host
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRepositories();

            services.AddMiddlewareService(Configuration);

            services.AddHealthChecks();

            services.AddHandlers();

            services.AddLoanConfiguration(Configuration);

            services.AddValidators();

            services.AddLoansDbContext(Configuration);
            
            services.AddKafkaService(Configuration);

            services.AddLoggerService(Configuration);

            services.AddSwaggerGen();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            
            app.UseSwaggerUI();

            app.UseExceptionHandler("/Error");

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseKafkaBus(lifetime);

            app.UseMiddleware<HeaderMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/", () => "Hello World. I am Danil!");
                
                endpoints.MapControllers();
            });
        }
    }
}
