using DbUp;
using Microsoft.Extensions.Configuration;

const string databaseConnectionStringSection = "DatabaseConnectionString";
const string defaultConnection = "DefaultConnection";

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Console.WriteLine("Миграция началась");

var result =
    DeployChanges.To
        .PostgresqlDatabase(configuration.GetSection(databaseConnectionStringSection)[defaultConnection])
        .WithScriptsFromFileSystem("Scripts")
        .LogToConsole()
        .Build()
        .PerformUpgrade();

Console.WriteLine($"Миграция закончилась успешно: {result.Successful}");

        