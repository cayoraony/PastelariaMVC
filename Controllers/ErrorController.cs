using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
