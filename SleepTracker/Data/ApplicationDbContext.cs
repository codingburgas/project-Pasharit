using Microsoft.EntityFrameworkCore;
using SleepTracker.Models;

namespace SleepTracker.Data
{
    // Database context class used by Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        // Constructor with configuration options (connection string, etc.)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<SleepLog> SleepLogs { get; set; }
        public DbSet<SleepFactor> SleepFactors { get; set; }

        // Configure relationships between entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One User -> Many SleepLogs
            modelBuilder.Entity<User>()
                .HasMany(u => u.SleepLogs)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One SleepLog -> Many Factors
            modelBuilder.Entity<SleepLog>()
                .HasMany(s => s.Factors)
                .WithOne(f => f.SleepLog)
                .HasForeignKey(f => f.SleepLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}