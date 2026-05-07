using Microsoft.EntityFrameworkCore;


namespace Expense_Tracker.Models
{
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DatabaseContext(Microsoft.EntityFrameworkCore.DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Signup> Signups { get; set; }
    }
}
