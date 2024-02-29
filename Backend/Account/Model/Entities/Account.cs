using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account.Model.Entities
{
    [Table("Accounts")] // Optional: Explicitly maps to the "Accounts" table
    public record Account
    {
        [Key] // Marks AccId as the primary key
        public Guid AccId { get; set; }

        [Required] 
        [EmailAddress] 
        public string Email { get; set; }

        [Required] // Marks PasswordHashed as a required field
        public string PasswordHashed { get; set; }

        [Column(TypeName = "decimal(18, 2)")] // Specifies the precision for Balance
        public decimal Balance { get; set; } 

        
    }
}