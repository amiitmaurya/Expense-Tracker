using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class LoginSignup
    {
        public Login? LoginForm { get; set; }
        public Signup? SignupForm { get; set; }
    }
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class Signup
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
