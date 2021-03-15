using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PastelariaMvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Login");
        }
    }
}
