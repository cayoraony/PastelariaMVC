using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PastelariaMvc.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() {}

        public int GetSessionId(string token)
        {
            return DecodeToken.getId(token);
        }

        public bool GetSessionEGestor(string token)
        {
            return DecodeToken.getEGestor(token);
        }

        public IActionResult RedirectBasedOnToken() 
        {
            var token = HttpContext.Session.GetString("Token");

            if(token == null)
            {
                return View();
            }

            if(GetSessionEGestor(token))
            {
                return RedirectToAction("HomeGestor", "Usuario", new { id = GetSessionId(token) });
            }
            else
            {
                return RedirectToAction("Listar", "Tarefa", new { id = GetSessionId(token) });
            }
        }
    }
}
