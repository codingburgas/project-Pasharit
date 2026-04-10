using SleepTracker.Models;

namespace SleepTracker.Services
{
    public class SleepService : ISleepService
    {
        public double CalculateDurationHours(SleepLog sleepLog)
        {
            return (sleepLog.SleepEnd - sleepLog.SleepStart).TotalHours;
        }

        public bool IsSleepLogValid(SleepLog sleepLog)
        {
            if (sleepLog.SleepStart > DateTime.Now) return false;
            if (sleepLog.SleepEnd > DateTime.Now) return false;
            if (sleepLog.SleepEnd <= sleepLog.SleepStart) return false;

            return true;
        }
    }
}