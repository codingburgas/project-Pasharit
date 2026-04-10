using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.Models;

namespace SleepTracker.Controllers
{
    public class SleepLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SleepLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sleepLogs = await _context.SleepLogs
                .Include(s => s.User)
                .ToListAsync();

            return View(sleepLogs);
        }

        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SleepLog sleepLog)
        {
            if (ModelState.IsValid)
            {
                _context.SleepLogs.Add(sleepLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", sleepLog.UserId);
            return View(sleepLog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sleepLog = await _context.SleepLogs.FindAsync(id);
            if (sleepLog == null) return NotFound();

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", sleepLog.UserId);
            return View(sleepLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SleepLog sleepLog)
        {
            if (id != sleepLog.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(sleepLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", sleepLog.UserId);
            return View(sleepLog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sleepLog = await _context.SleepLogs
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sleepLog == null) return NotFound();

            return View(sleepLog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sleepLog = await _context.SleepLogs.FindAsync(id);
            if (sleepLog != null)
            {
                _context.SleepLogs.Remove(sleepLog);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sleepLog = await _context.SleepLogs
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sleepLog == null) return NotFound();

            return View(sleepLog);
        }
    }
}