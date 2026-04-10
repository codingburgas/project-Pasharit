namespace DefaultNamespace;

public class SleepFactor : BaseEntity
{
    public string Name { get; set; }

    public int SleepLogId { get; set; }
    public SleepLog SleepLog { get; set; }
}