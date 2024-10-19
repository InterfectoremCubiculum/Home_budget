using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_library.Models
{
    public class HomeBudgetDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeCategory> IncomeCategories {get; set;}
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Income-Category Many To Many
            modelBuilder.Entity<IncomeCategory>()
                .HasKey(ic => new { ic.IncomeId, ic.CategoryId });

            modelBuilder.Entity<IncomeCategory>()
                .HasOne<Income>()
                .WithMany()
                .HasForeignKey(ic => ic.IncomeId);

            modelBuilder.Entity<IncomeCategory>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(ic => ic.CategoryId);

            //Expanse-Category Many To Many
            modelBuilder.Entity<ExpenseCategory>()
              .HasKey(ec => new { ec.ExpenseId, ec.CategoryId });

            modelBuilder.Entity<ExpenseCategory>()
                .HasOne<Expense>()
                .WithMany()
                .HasForeignKey(ec => ec.ExpenseId);

            modelBuilder.Entity<ExpenseCategory>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(ec => ec.CategoryId);

            //User-Income One To Many
            modelBuilder.Entity<Income>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.UserID);

            //User-Expense One To Many
            modelBuilder.Entity<Expense>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.UserID);

            //Decimal precision
            modelBuilder.Entity<Expense>()
                .Property(e => e.Value)
                .HasColumnType("decimal(18,2)");

            //Decimal precision
            modelBuilder.Entity<Income>()
               .Property(e => e.Value)
               .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HomeBudgetDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
