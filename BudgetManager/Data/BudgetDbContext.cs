using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Data
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<MonthPattern> MonthPatterns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthPattern>()
                .HasOne(x => x.Pattern)
                .WithMany(p => p.MonthPatterns)
                .HasForeignKey(f => f.PatternId);
        }
    }
}