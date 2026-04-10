using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.ViewModels;

namespace SleepTracker.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var last7Days = now.AddDays(-7);
            var last30Days = now.AddDays(-30);

            var sleepLogs = await _context.SleepLogs
                .Include(s => s.User)
                .Include(s => s.Factors)
                .ToListAsync();

            var avg7 = sleepLogs
                .Where(s => s.SleepStart >= last7Days)
                .Select(s => s.DurationHours)
                .DefaultIfEmpty(0)
                .Average();

            var avg30 = sleepLogs
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

            var consistencyPoints = sleepLogs
                .OrderBy(s => s.SleepStart)
                .Select(s => new SleepConsistencyPoint
                {
                    DateLabel = s.SleepStart.ToString("dd.MM"),
                    BedTimeHour = s.SleepStart.Hour + s.SleepStart.Minute / 60.0
                })
                .ToList();

            var factorAnalysis = sleepLogs
                .SelectMany(s => s.Factors.Select(f => new { f.Name, f.Value, s.Quality }))
                .GroupBy(x => new { x.Name, x.Value })
                .Select(g => new FactorAnalysisItem
                {
                    FactorName = g.Key.Name,
                    FactorValue = g.Key.Value,
                    AverageQuality = g.Average(x => x.Quality),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.AverageQuality)
                .ThenByDescending(x => x.Count)
                .ToList();

            var model = new StatisticsViewModel
            {
                AverageLast7Days = avg7,
                AverageLast30Days = avg30,
                BestNightInfo = bestNight == null
                    ? "No data"
                    : $"{bestNight.SleepStart:dd.MM.yyyy HH:mm} | Quality: {bestNight.Quality} | Duration: {bestNight.DurationHours:F2} h",
                WorstNightInfo = worstNight == null
                    ? "No data"
                    : $"{worstNight.SleepStart:dd.MM.yyyy HH:mm} | Quality: {worstNight.Quality} | Duration: {worstNight.DurationHours:F2} h",
                ConsistencyPoints = consistencyPoints,
                FactorAnalysis = factorAnalysis
            };

            return View(model);
        }
    }
}