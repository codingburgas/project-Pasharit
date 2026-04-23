using SleepTracker.Models;

namespace SleepTracker.Services
{
    // Defines contract for sleep-related logic
    public interface ISleepService
    {
        double CalculateDurationHours(SleepLog sleepLog);

        bool IsSleepLogValid(SleepLog sleepLog);
    }
}