// Provides MVC controller functionality.
using Microsoft.AspNetCore.Mvc;

namespace _368_CIE_2.Controllers;

// Handles the application's home page and privacy page.
public class HomeController : Controller
{
    // Displays the application home page.
    public IActionResult Index()
    {
        // Redirect the user to the doctor list.
        return RedirectToAction("Index", "Doctor");
    }

    // Displays the privacy page.
    public IActionResult Privacy()
    {
        // Return the Privacy Razor View.
        return View();
    }
}
