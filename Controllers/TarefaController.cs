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
    public class TarefaController : Controller
    {
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {

            ApiConnection client = new ApiConnection("tarefa/criar");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, tarefa);
            
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        


        public async Task<IActionResult> ContarTarefas(int id)
        {
            ApiConnection client = new ApiConnection("usuario/" + id + "/tarefa/quantidade");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            string result;
            if(response.IsSuccessStatusCode){
                result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                return View(int.Parse(result));
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
            return View();
        }

        public async Task<IActionResult> ConsultarTarefasGestor(int id)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/" + id + "/tarefa/pendentes");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            TarefasGestorViewModel tarefas = new TarefasGestorViewModel();
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();
                return View(tarefas);
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }

            return View();
        }
    }
}
