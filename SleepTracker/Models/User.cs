namespace SleepTracker.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public List<SleepLog> SleepLogs { get; set; } = new List<SleepLog>();
    }
}