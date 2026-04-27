using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public  required string Title { get; set; }

        public decimal Amount { get; set; }

        public required string Category { get; set; } // Food, Travel, etc.

        public DateTime Date { get; set; }

        public required string Type { get; set; } // Income or Expense
    }
}
