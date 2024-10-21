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
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Transaction-Category Many To Many
            modelBuilder.Entity<TransactionCategory>()
                .HasKey(ic => new { ic.TransactionId, ic.CategoryId });

            modelBuilder.Entity<TransactionCategory>()
                .HasOne<Transaction>()
                .WithMany()
                .HasForeignKey(ic => ic.TransactionId);

            modelBuilder.Entity<TransactionCategory>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(ic => ic.CategoryId);



            //User-Transaction One To Many
            modelBuilder.Entity<Transaction>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.UserID);

            //Decimal precision
            modelBuilder.Entity<Transaction>()
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
