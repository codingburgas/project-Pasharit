namespace SleepTracker.DTOs
{
    // DTO for sending SleepLog data to the UI
    public class SleepLogDTO
    {
        public int Id { get; set; }

        public DateTime SleepStart { get; set; }
        public DateTime SleepEnd { get; set; }
        public int Quality { get; set; }
        public string UserName { get; set; } = string.Empty;
        public double DurationHours { get; set; }
    }
}