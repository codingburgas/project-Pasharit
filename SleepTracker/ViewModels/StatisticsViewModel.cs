namespace SleepTracker.ViewModels
{
    public class StatisticsViewModel
    {
        public double AverageLast7Days { get; set; }
        public double AverageLast30Days { get; set; }

        public string BestNightInfo { get; set; } = string.Empty;
        public string WorstNightInfo { get; set; } = string.Empty;

        public List<SleepConsistencyPoint> ConsistencyPoints { get; set; } = new();
        public List<FactorAnalysisItem> FactorAnalysis { get; set; } = new();
    }

    public class SleepConsistencyPoint
    {
        public string DateLabel { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public double BedTimeHour { get; set; }
    }

    public class FactorAnalysisItem
    {
        public string FactorName { get; set; } = string.Empty;
        public string FactorValue { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public double AverageQuality { get; set; }
        public int Count { get; set; }
    }
}