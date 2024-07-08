using System.Collections;
using Loans.Application.AppServices.Contracts.Clients.Models;

namespace Loans.Application.AppServices.UnitTests.Clients.Data
{
    internal class InvalidClientInternalRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ClientInternalRequest
                {
                    LastName = "Петрушкин",
                    MiddleName = "Котович",
                    BirthDate = new DateTime(2002, 03, 03),
                }
            };
            
            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Дима",
                    MiddleName = "Котович",
                    BirthDate = new DateTime(2002, 05, 05),
                }
            };

            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Андрей",
                    LastName = "Петрушкин",
                    MiddleName = "Серг234еевич",
                    BirthDate = new DateTime(2000, 01, 02),
                }
            };

            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Настя",
                    LastName = "Настович",
                    MiddleName = "Котеева",
                }
            };
            
            yield return new object[]
            {
                new ClientInternalRequest
                {
                    FirstName = "Настя",
                    LastName = "Настович",
                    MiddleName = "Котеева",
                    BirthDate = new DateTime(2017, 09, 09),
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}