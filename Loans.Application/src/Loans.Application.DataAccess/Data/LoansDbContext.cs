using Loans.Application.DataAccess.Clients.Models;
using Loans.Application.DataAccess.Loans.Models;
using Microsoft.EntityFrameworkCore;

namespace Loans.Application.DataAccess.Data
{
    internal class LoansDbContext : DbContext
    {
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<LoanContractEntity> LoanContracts { get; set; }

        public LoansDbContext(DbContextOptions options)
            : base(options)
        {
        }

    }
}