using Loans.Application.AppServices.Clients.Repository;
using Loans.Application.AppServices.Contracts.Infrastructure.Exceptions;
using Loans.Application.AppServices.Contracts.Loans.Configuration;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.AppServices.Contracts.Loans.Validators;
using Microsoft.Extensions.Options;

namespace Loans.Application.AppServices.Loans.Validators
{
    /// <inheritdoc />
    internal class LoanValidator : ILoanValidator
    {
        private readonly LoanSpecification _configurationParameters;
        
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса LoanValidator с указанными параметрами конфигурации и репозиторием клиентов.
        /// </summary>
        /// <param name="configurationParameters">Параметры конфигурации для валидации кредитных договоров.</param>
        /// <param name="clientRepository">Репозиторий для работы с клиентами.</param>
        public LoanValidator(IOptionsMonitor<LoanSpecification> configurationParameters, IClientRepository clientRepository)
        {
            _configurationParameters = configurationParameters.CurrentValue;
            _clientRepository = clientRepository;
        }
        
        public void Validate(LoanContractInternalRequest request)
        {
            var validationErrors = new List<string>();

            var client = _clientRepository.GetClientById(request.ClientId, CancellationToken.None).GetAwaiter().GetResult();

            if (client == null)
            {
                validationErrors.Add($"Клиента с Id {request.ClientId} не существует");
            }
            else if (client.Salary < _configurationParameters.MinSalary)
            {
                validationErrors.Add($"Минимальная зарплата для кредита: {_configurationParameters.MinSalary}");
            }

            if (request.Amount < _configurationParameters.MinLoanAmount)
            {
                validationErrors.Add($"Минимальная сумма кредита {_configurationParameters.MinLoanAmount}.");
            }

            if (request.Amount > _configurationParameters.MaxLoanAmount)
            {
                validationErrors.Add($"Максимальная сумма кредита {_configurationParameters.MaxLoanAmount}.");
            }

            if (request.LoanTermMonth < _configurationParameters.MinLoanTermMonth)
            {
                validationErrors.Add($"Минимальный срок кредита {_configurationParameters.MinLoanTermMonth}.");
            }

            if (request.LoanTermMonth > _configurationParameters.MaxLoanTermMonth)
            {
                validationErrors.Add($"Максимальный срок кредита {_configurationParameters.MaxLoanTermMonth}.");
            }

            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }
        }
    }
}
