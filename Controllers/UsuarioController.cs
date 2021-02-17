using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;

namespace PastelariaMvc.Controllers
{
    public class UsuarioController : Controller
    {

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

                ApiConnection clientParaTotal = new ApiConnection($"usuario/gestor/{idGestor}/tarefa/total");
                HttpResponseMessage responseParaTotal = await clientParaTotal.Client.GetAsync(clientParaTotal.Url);
                if (responseParaTotal.IsSuccessStatusCode)
                {
                    subordinadosResult.QteTotalTarefas = int.Parse(await responseParaTotal.Content.ReadAsStringAsync());
                    
                    clientParaTotal.Close();
                    return View(subordinadosResult);
                }
            }
            Console.WriteLine(response.StatusCode);  
            return View();
        }


        public async Task<IActionResult> CriarSubordinado([FromForm]Usuario usuario)
        {
            ApiConnection client = new ApiConnection("usuario/subordinado/criar");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Index", "Home");   
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }
    }
}