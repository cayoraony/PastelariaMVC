using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMVC.ViewModel;

namespace PastelariaMVC.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoginGestor(Usuario gestor)
        {
            Console.WriteLine(gestor);
            ApiConnection client = new ApiConnection("usuario/gestor/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, gestor);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction(nameof(Index));    
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }

        public async Task<IActionResult> ConsultarUsuariosGestor([FromQuery] int idGestor)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/"+idGestor+"/subordinados");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            GestorHomeViewModel subordinadosResult = new GestorHomeViewModel();

            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                subordinadosResult.Subordinados = JsonConvert.DeserializeObject<List<Usuario>>(result);
                client.Close();
                return View(subordinadosResult);
            }
            Console.WriteLine(response.StatusCode);  
            return View();
        }

        public async Task<IActionResult> AtualizarSubordinado(int id, AtualizarUsuarioViewModel usuario)
        {
            ApiConnection client = new ApiConnection("usuario/"+id+"/atualizar");
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
           
            if (response.IsSuccessStatusCode)
            {
                
                client.Close();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> AtivarDesativar(int id)
        {
            var requestBody = "";
            ApiConnection client = new ApiConnection($"usuario/{id}/status");
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
           
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}