using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SleepTracker.Models;

namespace SleepTracker.Controllers;

// Controller for basic pages (Home, Privacy, Error)
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Shows error page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel 
        { 
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
        });
    }
}