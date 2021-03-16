using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            // return RedirectBasedOnToken();

            // TODO: Remover encademaneto de if
            if(HttpContext.Session.GetString("Token") == null)
            {
                return View();
            }
            // // TODO: Mover variáveis de acesso as informações do usuário para o base controller
            // var token = HttpContext.Session.GetString("Token");
            // var idUsuario = DecodeToken.getId(token);
            // var eGestor = DecodeToken.getEGestor(token);

            Session session = GetSession();
            if (session.eGestor)
            {
                return RedirectToAction("HomeGestor", "Usuario", new { id = session.idUsuario });
            }
            else // TODO: Esse if é inutil
            {
                return RedirectToAction("Listar", "Tarefa", new { id = session.idUsuario });
            }
        }

        // TODO: Melhorar esse metodo como um todo
        [HttpPost]
        public async Task<IActionResult> LoginPost(Usuario usuario)
        {
            
            if(HttpContext.Session.GetString("Token") != null)
            {
                Session session = GetSession();
                if (session.eGestor)
                {
                    return RedirectToAction("HomeGestor", "Usuario", new { id = session.idUsuario });
                }
                else
                {
                    return RedirectToAction("Listar", "Tarefa", new { id = session.idUsuario });
                }
            }

            ApiConnection client = new ApiConnection("usuario/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                // // TODO: Criar um método genérico pra fazer a leitura
                // string fulljson = await response.Content.ReadAsStringAsync();
                // LoginTokenViewModel usuariologado = new LoginTokenViewModel();
                // usuariologado = JsonConvert.DeserializeObject<LoginTokenViewModel>(fulljson);

                // LoginTokenViewModel usuariologado = new LoginTokenViewModel();
                // usuariologado = DeserializeObject<LoginTokenViewModel>(response).Result;

                // Duvida: é indicado usar o .Result? 
                HttpContext.Session.SetString("Token", DeserializeObject<LoginTokenViewModel>(response).Result.Token);
                Session session = GetSession();
                client.Close();
                if (session.eGestor)
                {
                    return RedirectToAction("HomeGestor", "Usuario", new { id = session.idUsuario });
                }
                else
                {
                    return RedirectToAction("Listar", "Tarefa", new { id = session.idUsuario });
                }
            }

            // ToDo - JM
            // Fazer alguma notificação ou encaminhar para página que mostre senha errada
            return RedirectToAction("Login", "Login");
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}