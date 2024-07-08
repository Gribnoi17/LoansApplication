using Loans.Application.DataAccess.Clients.Models;

namespace Loans.Application.DataAccess.UnitTests.Clients.Data
{
    internal class ClientEntityTestData
    {
        private List<ClientEntity> _testClientsEntities;

        public List<ClientEntity> GetTestClientEntities()
        {
            if (_testClientsEntities == null)
            {
                _testClientsEntities = InitializeClientEntitiesTestData();
            }

            return _testClientsEntities.ToList();
        }

        private List<ClientEntity> InitializeClientEntitiesTestData()
        {
            var clientsEntities = new List<ClientEntity>();

            clientsEntities.Add(new ClientEntity { Id = 1, FirstName = "Данил", LastName = "Китов", BirthDate = new DateOnly(2000, 01, 04), Salary = 120000 });
            clientsEntities.Add(new ClientEntity { Id = 2, FirstName = "Данил", LastName = "Петров", MiddleName = "Кириллович", BirthDate = new DateOnly(1999, 02, 01), Salary = 90000 });
            clientsEntities.Add(new ClientEntity { Id = 3, FirstName = "Арсений", LastName = "Морозов", MiddleName = "Денисович", BirthDate = new DateOnly(2001, 06, 09), Salary = 180000 });
            clientsEntities.Add(new ClientEntity { Id = 4, FirstName = "Георгий", LastName = "Китов", MiddleName = "Петрович", BirthDate = new DateOnly(1998, 02, 01), Salary = 90000 });

            return clientsEntities;
        }
    }
}