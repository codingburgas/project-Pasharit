using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SleepTracker.Models
{
    // Represents a user in the system
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";

        // One User -> many SleepLogs
        [ValidateNever]
        public List<SleepLog> SleepLogs { get; set; } = new();
    }
}