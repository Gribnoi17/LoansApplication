using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.DataAccess.UnitTests.Clients.Data
{
    internal class ClientTestData
    {
        private List<Client> _testClients;

        public List<Client> GetTestClients()
        {
            if (_testClients == null)
            {
                _testClients = InitializeClientsTestData();
            }

            return _testClients.ToList();
        }

        private List<Client> InitializeClientsTestData()
        {
            var clients = new List<Client>();

            clients.Add(new Client { Id = 1, FirstName = "Данил", LastName = "Китов", BirthDate = new DateTime(2000, 01, 04), Salary = 120000 });
            clients.Add(new Client { Id = 2, FirstName = "Данил", LastName = "Петров", MiddleName = "Кириллович", BirthDate = new DateTime(1999, 02, 01), Salary = 90000 });
            clients.Add(new Client { Id = 3, FirstName = "Арсений", LastName = "Морозов", MiddleName = "Денисович", BirthDate = new DateTime(2001, 06, 09), Salary = 180000 });
            clients.Add(new Client { Id = 4, FirstName = "Георгий", LastName = "Китов", MiddleName = "Петрович", BirthDate = new DateTime(1998, 02, 01), Salary = 90000 });

            return clients;
        }
    }
}