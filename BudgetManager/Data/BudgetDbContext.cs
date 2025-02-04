﻿using BudgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Data
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Pattern> Patterns { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<MonthPattern> MonthPatterns { get; set; }
        public virtual DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public virtual DbSet<RecurringTransactionSchedule> RecurringTransactionSchedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthPattern>()
                .HasOne(x => x.Pattern)
                .WithMany(p => p.MonthPatterns)
                .HasForeignKey(f => f.PatternId);

            modelBuilder.Entity<RecurringTransaction>()
                .HasOne(x => x.Schedule)
                .WithOne(rt => rt.RecurringTransaction)
                .HasForeignKey<RecurringTransactionSchedule>(s => s.RecurringTransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}