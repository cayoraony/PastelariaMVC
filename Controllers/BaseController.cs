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
        public string token { get; set; }
        public int idUsuario { get; set; }
        public bool eGestor { get; set; }

        public BaseController()
        {
            this.token = HttpContext.Session.GetString("Token");
            this.idUsuario = DecodeToken.getId(this.token);
            this.eGestor = DecodeToken.getEGestor(this.token);
        }

        public IActionResult RedirectBasedOnToken() 
        {
            if(token == null)
            {
                return View();
            }

            if(eGestor)
            {
                return RedirectToAction("HomeGestor", "Usuario", new { id = idUsuario });
            }
            else
            {
                return RedirectToAction("Listar", "Tarefa", new { id = idUsuario });
            }

        }
    }
}
