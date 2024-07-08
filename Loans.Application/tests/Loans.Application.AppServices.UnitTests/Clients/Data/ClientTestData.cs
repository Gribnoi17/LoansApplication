using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.UnitTests.Clients.Data
{
    internal class ClientTestData
    {
        private List<Client> _testClients;

        public List<Client> GetTestClients()
        {
            if (_testClients == null)
            {
                _testClients = InitializeClientsTest();
            }

            return _testClients.ToList();
        }

        private List<Client> InitializeClientsTest()
        {
            var clients = new List<Client>();

            clients.Add(new Client { Id = 1, FirstName = "Данил", LastName = "Китов", BirthDate = new DateTime(2000, 01, 04), Salary = 120000 });
            clients.Add(new Client { Id = 2, FirstName = "Арсений", LastName = "Морозов", MiddleName = "Денисович", BirthDate = new DateTime(2001, 06, 09), Salary = 180000 });
            clients.Add(new Client { Id = 3, FirstName = "Данил", LastName = "Петров", MiddleName = "Кириллович", BirthDate = new DateTime(1999, 02, 01), Salary = 90000 });

            return clients;
        }
    }
}