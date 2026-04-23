using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.ViewModels;

namespace SleepTracker.Controllers
{
    // Controller for calculating and displaying sleep statistics
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Main statistics page
        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var last7Days = now.AddDays(-7);
            var last30Days = now.AddDays(-30);

            // Load sleep logs with related data
            var sleepLogs = await _context.SleepLogs
                .Include(s => s.User)
                .Include(s => s.Factors)
                .ToListAsync();

            var averageLast7Days = sleepLogs
                .Where(s => s.SleepStart >= last7Days)
                .Select(s => s.DurationHours)
                .DefaultIfEmpty(0)
                .Average();

            var averageLast30Days = sleepLogs
                .Where(s => s.SleepStart >= last30Days)
                .Select(s => s.DurationHours)
                .DefaultIfEmpty(0)
                .Average();

            var bestNight = sleepLogs
                .OrderByDescending(s => s.Quality)
                .ThenByDescending(s => s.DurationHours)
                .FirstOrDefault();

            var worstNight = sleepLogs
                .OrderBy(s => s.Quality)
                .ThenBy(s => s.DurationHours)
                .FirstOrDefault();

            // Data for sleep consistency
            var consistencyPoints = sleepLogs
                .OrderBy(s => s.SleepStart)
                .Select(s => new SleepConsistencyPoint
                {
                    DateLabel = s.SleepStart.ToString("dd.MM"),
                    UserName = s.User?.Name ?? "Unknown",
                    BedTimeHour = s.SleepStart.Hour + s.SleepStart.Minute / 60.0
                })
                .ToList();

            // Analyze factor impact on sleep quality
            var factorAnalysis = sleepLogs
                .SelectMany(s => s.Factors.Select(f => new
                {
                    f.Name,
                    f.Value,
                    UserName = s.User != null ? s.User.Name : "Unknown",
                    s.Quality
                }))
                .GroupBy(x => new { x.Name, x.Value, x.UserName })
                .Select(g => new FactorAnalysisItem
                {
                    FactorName = g.Key.Name,
                    FactorValue = g.Key.Value,
                    UserName = g.Key.UserName,
                    AverageQuality = g.Average(x => x.Quality),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.AverageQuality)
                .ThenByDescending(x => x.Count)
                .ToList();

            var model = new StatisticsViewModel
            {
                AverageLast7Days = averageLast7Days,
                AverageLast30Days = averageLast30Days,

                BestNightInfo = bestNight == null
                    ? "No data"
                    : $"{bestNight.SleepStart:dd.MM.yyyy HH:mm} | User: {bestNight.User?.Name} | Quality: {bestNight.Quality} | Duration: {bestNight.DurationHours:F2} h",

                WorstNightInfo = worstNight == null
                    ? "No data"
                    : $"{worstNight.SleepStart:dd.MM.yyyy HH:mm} | User: {worstNight.User?.Name} | Quality: {worstNight.Quality} | Duration: {worstNight.DurationHours:F2} h",

                ConsistencyPoints = consistencyPoints,
                FactorAnalysis = factorAnalysis
            };

            return View(model);
        }
    }
}