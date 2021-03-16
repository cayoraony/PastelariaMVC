using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PastelariaMvc.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PastelariaMvc.Models;

namespace PastelariaMvc.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() {}

        public Session GetSession()
        {
            var getToken = HttpContext.Session.GetString("Token");
            Session session = new Session();
            
            session.token = getToken;
            session.idUsuario = DecodeToken.getId(getToken);
            session.eGestor = DecodeToken.getEGestor(getToken);
            session.nomeUsuario = DecodeToken.getNome(getToken);

            return session;
        }

        // Duvida: por que isso desencadeou "localhost redirected you too many times"
        // public IActionResult RedirectBasedOnToken() 
        // {
        //     Session session = GetSession();

        //     if(session.token == null)
        //     {
        //         return RedirectToAction("Login", "Login");
        //     }

        //     if(session.eGestor)
        //     {
        //         return RedirectToAction("HomeGestor", "Usuario", new { id = session.idUsuario });
        //     }
        //     else
        //     {
        //         return RedirectToAction("Listar", "Tarefa", new { id = session.idUsuario });
        //     }
        // }

        public async Task<T> DeserializeObject<T>(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();
            // object result = JsonConvert.DeserializeObject<T>(json);
            // return (T) Convert.ChangeType(result, typeof(T));
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<IActionResult> VerificarErroAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
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