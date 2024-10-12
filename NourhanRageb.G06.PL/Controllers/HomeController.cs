using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NourhanRageb.G06.PL.Models;
using System.Diagnostics;

namespace NourhanRageb.G06.PL.Controllers
{
    [Authorize]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
