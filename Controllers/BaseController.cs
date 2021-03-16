using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PastelariaMvc.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public async Task<IActionResult> VerificarErroAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                Console.WriteLine("Unauthorized ou Forbidden");
                return RedirectToAction("Login", "Login");
            }
            else
            {
                Console.WriteLine("else");
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }
    }
}
