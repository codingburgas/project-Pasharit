using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SleepTracker.Models
{
    // Main entity representing a sleep record
    public class SleepLog : BaseEntity
    {
        [Required]
        public DateTime SleepStart { get; set; }

        [Required]
        public DateTime SleepEnd { get; set; }

        [Range(1, 5)]
        public int Quality { get; set; }

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ValidateNever]
        public User? User { get; set; }

        // One SleepLog -> many Factors
        [ValidateNever]
        public List<SleepFactor> Factors { get; set; } = new();

        // Calculated duration (not stored in DB)
        public double DurationHours => (SleepEnd - SleepStart).TotalHours;
    }
}