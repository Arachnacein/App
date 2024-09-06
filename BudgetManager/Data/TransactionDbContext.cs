using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
    }
}
