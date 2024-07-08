using System.Collections;
using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.UnitTests.Clients.Data
{
    internal class ValidClientInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Иван",
                    LastName = "Иванович",
                    MiddleName = "Котович",
                    BirthDate = new DateTime(2002, 03, 03),
                }
            };

            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Саша",
                    LastName = "Сашевич",
                    MiddleName = "Сергеевич",
                    BirthDate = new DateTime(2000, 01, 02),
                }
            };

            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Катя",
                    LastName = "Машинович",
                    MiddleName = "Котеева",
                    BirthDate = new DateTime(2003, 08, 10),
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}