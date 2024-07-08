using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loans.Application.DataAccess.Clients.Models
{
    [Table("clients", Schema = "dcs_loans")]
    internal class ClientEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        
        [Column("first_name")]
        [Required]
        public string FirstName { get; set; }
        
        [Column("last_name")]
        [Required]
        public string LastName { get; set; }
        
        [Column("middle_name")]
        public string? MiddleName { get; set; }
        
        [Column("birth_date")]
        [Required]
        public DateOnly BirthDate { get; set; }

        [Column("salary")]
        [Required]
        public decimal Salary { get; set; }
    }
}