using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class UsuarioController : Controller
    {
        public async Task<IActionResult> ConsultarGestor(int id)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/" + id);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario gestor;
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                gestor = JsonConvert.DeserializeObject<Usuario>(result);
                client.Close();
                return View(new UsuarioViewModel { Usuario = gestor });
            }
            return View();
        }

        public async Task<IActionResult> AtualizarGestor(int id, AtualizarUsuarioViewModel usuario)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/" + id + "/atualizar");
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
            
           
            if (response.IsSuccessStatusCode)
            {
                
                client.Close();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }

        public async Task<IActionResult> LoginSubordinado(Usuario subordinado)
        {
            
            ApiConnection client = new ApiConnection("usuario/subordinado/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, subordinado);
                
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return View("~/Views/Home/Index.cshtml");
            }
            Console.WriteLine(response.StatusCode);
            
            
            return View();
        }
    }
}
