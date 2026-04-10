namespace SleepTracker.Models

public class User : BaseEntity
{
    public string Name { get; set; }

    public List<SleepLog> SleepLogs { get; set; }
}