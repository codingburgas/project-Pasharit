using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.Models;

namespace SleepTracker.Controllers
{
    // Controller responsible for managing Sleep Factors (CRUD operations)
    public class FactorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject database context
        public FactorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all factors with related SleepLog and User
        public async Task<IActionResult> Index()
        {
            var factors = await _context.SleepFactors
                .Include(f => f.SleepLog)
                .ThenInclude(s => s.User)
                .ToListAsync();

            return View(factors);
        }

        // Show Create form and prepare dropdown with SleepLogs
        public IActionResult Create()
        {
            var sleepLogs = _context.SleepLogs
                .Include(s => s.User)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    // Display text for dropdown (Id + User + Date)
                    Display = $"#{s.Id} | {s.User?.Name} | {s.SleepStart:dd.MM.yyyy HH:mm}"
                })
                .ToList();

            ViewBag.SleepLogs = new SelectList(sleepLogs, "Id", "Display");
            return View(new SleepFactor());
        }

        // Handle form submission for creating a factor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SleepFactor factor)
        {
            // Validate if selected SleepLog exists
            if (!_context.SleepLogs.Any(s => s.Id == factor.SleepLogId))
            {
                ModelState.AddModelError("SleepLogId", "Please select a valid sleep log.");
            }

            if (ModelState.IsValid)
            {
                _context.SleepFactors.Add(factor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            var sleepLogs = _context.SleepLogs
                .Include(s => s.User)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    Display = $"#{s.Id} | {s.User?.Name} | {s.SleepStart:dd.MM.yyyy HH:mm}"
                })
                .ToList();

            ViewBag.SleepLogs = new SelectList(sleepLogs, "Id", "Display", factor.SleepLogId);
            return View(factor);
        }

        // Show Edit form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var factor = await _context.SleepFactors.FindAsync(id);
            if (factor == null) return NotFound();

            var sleepLogs = _context.SleepLogs
                .Include(s => s.User)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    Display = $"#{s.Id} | {s.User?.Name} | {s.SleepStart:dd.MM.yyyy HH:mm}"
                })
                .ToList();

            ViewBag.SleepLogs = new SelectList(sleepLogs, "Id", "Display", factor.SleepLogId);
            return View(factor);
        }

        // Handle updating existing factor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SleepFactor factor)
        {
            if (id != factor.Id) return NotFound();

            // Validate SleepLog existence
            if (!_context.SleepLogs.Any(s => s.Id == factor.SleepLogId))
            {
                ModelState.AddModelError("SleepLogId", "Please select a valid sleep log.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(factor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            var sleepLogs = _context.SleepLogs
                .Include(s => s.User)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    Display = $"#{s.Id} | {s.User?.Name} | {s.SleepStart:dd.MM.yyyy HH:mm}"
                })
                .ToList();

            ViewBag.SleepLogs = new SelectList(sleepLogs, "Id", "Display", factor.SleepLogId);
            return View(factor);
        }

        // Show Delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var factor = await _context.SleepFactors
                .Include(f => f.SleepLog)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factor == null) return NotFound();

            return View(factor);
        }

        // Show detailed information about a factor
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var factor = await _context.SleepFactors
                .Include(f => f.SleepLog)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factor == null) return NotFound();

            return View(factor);
        }

        // Confirm deletion of a factor
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factor = await _context.SleepFactors.FindAsync(id);

            if (factor != null)
            {
                _context.SleepFactors.Remove(factor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}