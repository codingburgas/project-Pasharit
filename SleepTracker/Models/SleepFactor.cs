using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SleepTracker.Models
{
    // Represents a factor affecting sleep
    public class SleepFactor : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Value { get; set; } = string.Empty;

        // Foreign key to SleepLog
        [Required]
        public int SleepLogId { get; set; }

        [ValidateNever]
        public SleepLog? SleepLog { get; set; }
    }
}