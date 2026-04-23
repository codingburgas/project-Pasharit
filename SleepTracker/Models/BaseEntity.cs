namespace SleepTracker.Models
{
    // Base class for all entities in the system
    // Used to avoid code duplication (inheritance)
    public class BaseEntity
    {
        // Unique identifier for each record
        public int Id { get; set; }

        // Automatically stores creation date
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}