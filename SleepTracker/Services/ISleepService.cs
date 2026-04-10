using SleepTracker.Models;

namespace SleepTracker.Services
{
    public interface ISleepService
    {
        double CalculateDurationHours(SleepLog sleepLog);
        bool IsSleepLogValid(SleepLog sleepLog);
    }
}