using System.ComponentModel.DataAnnotations;

namespace SleepTracker.Models
{
    public class SleepLog : BaseEntity
    {
        [Required]
        public DateTime SleepStart { get; set; }

        [Required]
        public DateTime SleepEnd { get; set; }

        [Range(1, 5)]
        public int Quality { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<SleepFactor> Factors { get; set; } = new List<SleepFactor>();

        public double DurationHours => (SleepEnd - SleepStart).TotalHours;
    }
}