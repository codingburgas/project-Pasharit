namespace SleepTracker.Models
{
    public class SleepLog : BaseEntity
    {
        public DateTime SleepStart { get; set; }
        public DateTime SleepEnd { get; set; }
        public int Quality { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<SleepFactor> Factors { get; set; } = new List<SleepFactor>();
    }
}