using SleepTracker.Models;

namespace SleepTracker.Services
{
    // Service for sleep-related business logic
    public class SleepService : ISleepService
    {
        // Calculates sleep duration in hours
        public double CalculateDurationHours(SleepLog sleepLog)
        {
            return (sleepLog.SleepEnd - sleepLog.SleepStart).TotalHours;
        }

        // Validates sleep data (time correctness)
        public bool IsSleepLogValid(SleepLog sleepLog)
        {
            if (sleepLog.SleepStart > DateTime.Now) return false;
            if (sleepLog.SleepEnd > DateTime.Now) return false;
            if (sleepLog.SleepEnd <= sleepLog.SleepStart) return false;

            return true;
        }
    }
}