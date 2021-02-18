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
        public async Task<IActionResult> AlterarStatusDaTarefa(int id, AlterarStatusDaTarefaViewModel tarefa)
        {
            ApiConnection client = new ApiConnection($"tarefa/{id}/status");
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Index", "Home");
            }
            client.Close();
            return View();
        }

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
        public IActionResult Index()
        {
            return View();
        }

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
                return RedirectToAction("Index", "Home");
            }
            client.Close();
            return View();
        }

        public async Task<IActionResult> CriarComentario(Comentario comentario)
        {
            ApiConnection client = new ApiConnection("tarefa/comentario/criar");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, comentario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }

        public async Task<IActionResult> ConsultarComentarios(int id)
        {
            ApiConnection client = new ApiConnection("tarefa/"+id+"/comentarios");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            // List<Comentario> comentarioResult;
            ComentariosViewModel comentarioResult = new ComentariosViewModel();
            string result;
            // List<Comentario> result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                comentarioResult.Comentarios = JsonConvert.DeserializeObject<List<Comentario>>(result);
                client.Close();
                return View(comentarioResult);
            }
            Console.WriteLine(response.StatusCode);  
            return View();
        }

        public async Task<IActionResult> ConsultarTarefasGestorStatus([FromQuery] int idGestor, [FromQuery] int idStatus)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/"+idGestor+"/tarefa/status/"+idStatus);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            TarefasGestorStatusViewModel tarefasResult = new TarefasGestorStatusViewModel();
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefasResult.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();
                return View(tarefasResult);
            }
            Console.WriteLine(response.StatusCode);  
            return View();
        }

        public async Task<IActionResult> ConsultarTarefasUsuario(int id)
        {
            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/todas");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();
                return View(tarefas);
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }

        public async Task<IActionResult> Concluir(int id)
        {
            var requestBody = "";
            ApiConnection client = new ApiConnection("tarefa/"+id+"/concluir");
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