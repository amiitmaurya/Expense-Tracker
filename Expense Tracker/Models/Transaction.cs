using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Title { get; set; }

        public decimal Amount { get; set; }

        public required string Category { get; set; }

        public DateTime Date { get; set; }

        public required string Type { get; set; }

        public string? UserEmail { get; set; }
    }
}
