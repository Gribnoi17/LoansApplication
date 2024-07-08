using Loans.Application.AppServices.Contracts.Infrastructure.Validators;
using Loans.Application.AppServices.Contracts.Loans.Models;

namespace Loans.Application.AppServices.Contracts.Loans.Validators
{
    /// <inheritdoc />
    public interface ILoanValidator : IValidator<LoanContractInternalRequest>
    {
    }
}
