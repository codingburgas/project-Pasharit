using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.DTOs;
using SleepTracker.Models;
using SleepTracker.Services;

namespace SleepTracker.Controllers
{
    public class SleepLogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISleepService _sleepService;

        public SleepLogsController(ApplicationDbContext context, ISleepService sleepService)
        {
            _context = context;
            _sleepService = sleepService;
        }

        private bool IsAdmin()
        {
            var firstUser = _context.Users.FirstOrDefault();
            return firstUser != null && firstUser.Role == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            var sleepLogs = await _context.SleepLogs
                .Include(s => s.User)
                .Select(s => new SleepLogDTO
                {
                    Id = s.Id,
                    SleepStart = s.SleepStart,
                    SleepEnd = s.SleepEnd,
                    Quality = s.Quality,
                    UserName = s.User.Name,
                    DurationHours = (s.SleepEnd - s.SleepStart).TotalHours
                })
                .ToListAsync();

            ViewBag.IsAdmin = IsAdmin();

            return View(sleepLogs);
        }

        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name");
            return View(new SleepLog
            {
                SleepStart = DateTime.Now.AddHours(-8),
                SleepEnd = DateTime.Now,
                Quality = 3
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SleepLog sleepLog)
        {
            if (!_sleepService.IsSleepLogValid(sleepLog))
            {
                ModelState.AddModelError("", "Sleep start/end time is invalid.");
            }

            if (!_context.Users.Any(u => u.Id == sleepLog.UserId))
            {
                ModelState.AddModelError("UserId", "Please select a valid user.");
            }

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

            if (!_sleepService.IsSleepLogValid(sleepLog))
            {
                ModelState.AddModelError("", "Sleep start/end time is invalid.");
            }

            if (!_context.Users.Any(u => u.Id == sleepLog.UserId))
            {
                ModelState.AddModelError("UserId", "Please select a valid user.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sleepLog);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", sleepLog.UserId);
            return View(sleepLog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return Content("Only Admin can delete sleep logs.");
            }

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
            if (!IsAdmin())
            {
                return Content("Only Admin can delete sleep logs.");
            }

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