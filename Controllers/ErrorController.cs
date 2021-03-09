using Microsoft.AspNetCore.Mvc;

namespace PastelariaMvc.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(string Erro)
        {
            ViewBag.ErroMessage = Erro;
            return View();
        }
    }
}
