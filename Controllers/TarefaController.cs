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
    public class TarefaController : Controller
    {

        public async Task<IActionResult> CancelarTarefa([FromQuery]int idTarefa)
        {
            ApiConnection client = new ApiConnection($"tarefa/{idTarefa}/cancelar");
            //ta em patch na API tem q trocar pra get
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return View();
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }

        public async Task<IActionResult> ConsultarTarefasStatusUsuario([FromQuery] int idUsuario, [FromQuery] int idStatusTarefa)
        {
            ApiConnection client = new ApiConnection($"usuario/{idUsuario}/tarefa/status/{idStatusTarefa}");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            TarefasViewModel tarefasViewModel = new TarefasViewModel();

            string result;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefasViewModel.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();

                return View(tarefasViewModel);
            }

            return View();
        }

        public async Task<IActionResult> ConsultarTodasTarefasGestor([FromQuery] int idGestor)
        {
            ApiConnection client = new ApiConnection($"usuario/gestor/{idGestor}/tarefa/todas");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            TarefasViewModel tarefasViewModel = new TarefasViewModel();
            string result;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefasViewModel.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();

                return View(tarefasViewModel);
            }

            return View("index");
        }

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            ApiConnection client = new ApiConnection($"tarefa/{id}/datalimite");
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);

            if (response.IsSuccessStatusCode)
            {
                client.Close();
                Console.WriteLine(response.StatusCode);
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(response.StatusCode + " " + "= Merda");
            client.Close();
            return View();
        }
    }
}