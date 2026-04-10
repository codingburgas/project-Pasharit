using System.ComponentModel.DataAnnotations;

namespace SleepTracker.Models
{
    public class SleepFactor : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Value { get; set; } = string.Empty;
        // examples: Yes / No / Light / Heavy / 2 cups

        public int SleepLogId { get; set; }
        public SleepLog SleepLog { get; set; } = null!;
    }
}