using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Loans.Application.AppServices.Contracts.Loans;

namespace Loans.Application.DataAccess.Loans.Models
{
    [Table("loan_applications", Schema = "dcs_loans")]
    internal class LoanContractEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        
        [Column("client_id")]
        [Required]
        public long ClientId { get; set; }
        
        [Column("amount")]
        [Required]
        public decimal Amount { get; set; }
        
        [Column("loan_term_month")]
        [Required]
        public int LoanTermMonth { get; set; }
        
        [Column("interest_rate")]
        [Required]
        public decimal ExpectedInterestRate { get; set; }
        
        [Column("loan_date")]
        [Required]
        public DateOnly LoanDate { get; set; }
        
        [Column("status")]
        [Required]
        public LoanStatus Status { get; set; }
        
        [Column("rejection_reason")]
        public string? RejectionReason { get; set; }
    }
}