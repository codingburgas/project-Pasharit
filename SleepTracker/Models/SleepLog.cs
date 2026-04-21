using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [Required]
        public int UserId { get; set; }

        [ValidateNever]
        public User? User { get; set; }

        [ValidateNever]
        public List<SleepFactor> Factors { get; set; } = new();

        public double DurationHours => (SleepEnd - SleepStart).TotalHours;
    }
}