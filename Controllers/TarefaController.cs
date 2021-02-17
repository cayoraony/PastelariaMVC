using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMVC.Models;
using PastelariaMVC.ViewModel;

namespace PastelariaMVC.Controllers
{
    public class TarefaController : Controller
    {
        public IActionResult Index()
        {
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