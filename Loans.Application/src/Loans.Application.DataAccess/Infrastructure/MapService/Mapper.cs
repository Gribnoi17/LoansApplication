using Loans.Application.AppServices.Contracts.Clients.Models;
using Loans.Application.AppServices.Contracts.Loans.Models;
using Loans.Application.DataAccess.Clients.Models;
using Loans.Application.DataAccess.Loans.Models;

namespace Loans.Application.DataAccess.Infrastructure.MapService
{
    internal static class Mapper
    {
        public static ClientEntity MapToClientEntity(Client client)
        {
            return new ClientEntity
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = new DateOnly(client.BirthDate.Year, client.BirthDate.Month, client.BirthDate.Day),
                Salary = client.Salary
            };
        }
        
        public static Client MapToClient(ClientEntity clientEntity)
        {
            var client = new Client
            {
                Id = clientEntity.Id,
                FirstName = clientEntity.FirstName,
                LastName = clientEntity.LastName,
                MiddleName = clientEntity.MiddleName,
                BirthDate = new DateTime(clientEntity.BirthDate.Year, clientEntity.BirthDate.Month, clientEntity.BirthDate.Day),
                Salary = clientEntity.Salary
            };
            
            return client;
        }
        
        public static List<Client> MapToClients(List<ClientEntity> clientEntities)
        {
            var filteredClients = clientEntities.Select(clientEntity => 
                new Client
                {
                    Id = clientEntity.Id,
                    FirstName = clientEntity.FirstName,
                    LastName = clientEntity.LastName,
                    MiddleName = clientEntity.MiddleName,
                    BirthDate = new DateTime(clientEntity.BirthDate.Year, clientEntity.BirthDate.Month, clientEntity.BirthDate.Day),
                    Salary = clientEntity.Salary
                }).ToList();

            return filteredClients;
        }

        public static LoanContractEntity MapToLoanContractEntity(LoanContract loanContract)
        {
            return new LoanContractEntity
            {
                ClientId = loanContract.ClientId,
                Amount = loanContract.Amount,
                ExpectedInterestRate = loanContract.InterestRate,
                LoanDate = new DateOnly(loanContract.LoanDate.Year, loanContract.LoanDate.Month, loanContract.LoanDate.Day),
                LoanTermMonth = loanContract.LoanTermMonth,
                RejectionReason = loanContract.RejectionReason,
                Status = loanContract.Status
            };
        }
        
        public static LoanContract MapToLoanContract(LoanContractEntity loanContractEntity)
        {
            return new LoanContract
            {
                Id = loanContractEntity.Id,
                ClientId = loanContractEntity.ClientId,
                Amount = loanContractEntity.Amount,
                InterestRate = loanContractEntity.ExpectedInterestRate,
                LoanDate = new DateTime(loanContractEntity.LoanDate.Year, loanContractEntity.LoanDate.Month, loanContractEntity.LoanDate.Day),
                LoanTermMonth = loanContractEntity.LoanTermMonth,
                RejectionReason = loanContractEntity.RejectionReason,
                Status = loanContractEntity.Status
            };
        }
        
        public static List<LoanContract> MapToLoanContracts(List<LoanContractEntity> loanContractEntities)
        {
            var loanContracts = loanContractEntities.Select(loanContractEntity => 
                new LoanContract()
                {
                    Id = loanContractEntity.Id,
                    ClientId = loanContractEntity.ClientId,
                    Amount = loanContractEntity.Amount,
                    LoanTermMonth = loanContractEntity.LoanTermMonth,
                    InterestRate = loanContractEntity.ExpectedInterestRate,
                    LoanDate = new DateTime(loanContractEntity.LoanDate.Year, loanContractEntity.LoanDate.Month, loanContractEntity.LoanDate.Day),
                    Status = loanContractEntity.Status,
                    RejectionReason = loanContractEntity.RejectionReason
                }).ToList();

            return loanContracts;
        }
        
        public static void MapToUpdateClientEntity(Client client, ClientEntity clientEntity)
        {
            clientEntity.FirstName = client.FirstName;
            clientEntity.LastName = client.LastName;
            clientEntity.MiddleName = client.MiddleName;
            clientEntity.BirthDate = new DateOnly(client.BirthDate.Year, client.BirthDate.Month, client.BirthDate.Day);
            clientEntity.Salary = client.Salary;
        }
        
        public static void MapToUpdateLoanContractEntity(LoanContract loanContract, LoanContractEntity loanContractEntity)
        {
            loanContractEntity.Id = loanContract.Id;
            loanContractEntity.ClientId = loanContract.ClientId;
            loanContractEntity.Amount = loanContract.Amount;
            loanContractEntity.LoanTermMonth = loanContract.LoanTermMonth;
            loanContractEntity.ExpectedInterestRate = loanContract.InterestRate;
            loanContractEntity.LoanDate = new DateOnly(loanContract.LoanDate.Year, loanContract.LoanDate.Month, loanContract.LoanDate.Day);
            loanContractEntity.Status = loanContract.Status;
            loanContractEntity.RejectionReason = loanContract.RejectionReason;
        }
    }
}