using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class ChangePassword
    {
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string? NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password does not match")]
        public string? ConfirmPassword { get; set; }

    }
}
