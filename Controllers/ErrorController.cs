using Microsoft.AspNetCore.Mvc;
using System;

namespace PastelariaMvc.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Index(string Erro)
        {
            ViewBag.ErroMessage = Erro;
            return View();
        }
        public IActionResult Default()
        {
            ViewBag.ErroMessage = "Deu merda";
            Console.WriteLine("Chegou aqui 2");
            return View("Index");
        }
    }
}
