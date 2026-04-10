using Microsoft.EntityFrameworkCore;
using SleepTracker.Models;

namespace SleepTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SleepLog> SleepLogs { get; set; }
        public DbSet<SleepFactor> SleepFactors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SleepLogs)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SleepLog>()
                .HasMany(s => s.Factors)
                .WithOne(f => f.SleepLog)
                .HasForeignKey(f => f.SleepLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}