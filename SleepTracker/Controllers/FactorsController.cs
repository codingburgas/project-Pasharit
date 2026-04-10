using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.Models;

namespace SleepTracker.Controllers
{
    public class FactorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FactorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var factors = await _context.SleepFactors
                .Include(f => f.SleepLog)
                .ThenInclude(s => s.User)
                .ToListAsync();

            return View(factors);
        }

        public IActionResult Create()
        {
            ViewBag.SleepLogs = new SelectList(_context.SleepLogs.ToList(), "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SleepFactor factor)
        {
            if (ModelState.IsValid)
            {
                _context.SleepFactors.Add(factor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SleepLogs = new SelectList(_context.SleepLogs.ToList(), "Id", "Id", factor.SleepLogId);
            return View(factor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var factor = await _context.SleepFactors.FindAsync(id);
            if (factor == null) return NotFound();

            ViewBag.SleepLogs = new SelectList(_context.SleepLogs.ToList(), "Id", "Id", factor.SleepLogId);
            return View(factor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SleepFactor factor)
        {
            if (id != factor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(factor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SleepLogs = new SelectList(_context.SleepLogs.ToList(), "Id", "Id", factor.SleepLogId);
            return View(factor);
        }

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