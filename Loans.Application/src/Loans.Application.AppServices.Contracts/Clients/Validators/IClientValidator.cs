using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Infrastructure.Validators;

namespace Loans.Application.AppServices.Contracts.Clients.Validators
{
    /// <inheritdoc />
    public interface IClientValidator : IValidator<ClientInternalRequest>
    {
    }
}
