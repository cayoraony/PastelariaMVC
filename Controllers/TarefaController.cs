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
        // Provalvemente não será necessário usa-la, pois vai ter outros meios de alterar sem usar isso
        // public async Task<IActionResult> AlterarStatusDaTarefa(int id, AlterarStatusDaTarefaViewModel tarefa)
        // {
        //     ApiConnection client = new ApiConnection($"tarefa/{id}/status");
        //     HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         client.Close();
        //         return RedirectToAction("Index", "Home");
        //     }
        //     client.Close();
        //     return View();
        // }

        public async Task<IActionResult> Cancelar(int id)
        {
            var requestBody = "";
            ApiConnection client = new ApiConnection($"tarefa/{id}/cancelar");
            //ta em patch na API tem q trocar pra get
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                // ToDo - JM
                // Por enquanto esta estatico, porem depois que pegarmos
                // O id da session, mudará aqui
                //*************
                return RedirectToAction("Listar", "Tarefa", new { id = 1 });
            }
            return View();
        }

        // Ainda não está usando, mas pode usar nos Filtros
        // public async Task<IActionResult> ConsultarTarefasStatusUsuario([FromQuery] int idUsuario, [FromQuery] int idStatusTarefa)
        // {
        //     ApiConnection client = new ApiConnection($"usuario/{idUsuario}/tarefa/status/{idStatusTarefa}");
        //     HttpResponseMessage response = await client.Client.GetAsync(client.Url);

        //     TarefasViewModel tarefasViewModel = new TarefasViewModel();

        //     string result;

        //     if (response.IsSuccessStatusCode)
        //     {
        //         result = await response.Content.ReadAsStringAsync();
        //         tarefasViewModel.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
        //         client.Close();

        //         return View(tarefasViewModel);
        //         }

        //     return View();
        // }

        // public IActionResult Index()
        // {
        //     return View();
        // }

        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            ApiConnection client = new ApiConnection("tarefa/criar");
            Console.WriteLine(tarefa.IdStatusTarefa);
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, tarefa);
            
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Listar", "Tarefa", new { id = tarefa.IdGestor });
                // return RedirectToAction("Listar", "Tarefa", new {@id = id});
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }


        // public async Task<IActionResult> ConsultarTarefasGestor(int id)
        // {
        //     // ToDo - JM
        //     // Mudar a PROC para que tanto o gestor como subordinado
        //     // usem o mesmo endpoint
        //     // ******************
        //     ApiConnection client = new ApiConnection("usuario/gestor/" + id + "/tarefa/pendentes");
        //     HttpResponseMessage response = await client.Client.GetAsync(client.Url);
        //     TarefasGestorViewModel tarefas = new TarefasGestorViewModel();
        //     string result;
        //     if (response.IsSuccessStatusCode)
        //     {
        //         result = await response.Content.ReadAsStringAsync();
        //         tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
        //         client.Close();
        //         return View(tarefas);
        //     }
        //     else
        //     {
        //         Console.WriteLine(response.StatusCode);
        //     }

        //     return View();
        // }

        // public async Task<IActionResult> ConsultarTodasTarefasGestor([FromQuery] int idGestor)
        // {
        //     // ToDo - JM
        //     // Mudar a PROC para que tanto o gestor como subordinado
        //     // usem o mesmo endpoint
        //     // ******************
        //     ApiConnection client = new ApiConnection($"usuario/gestor/{idGestor}/tarefa/todas");
        //     HttpResponseMessage response = await client.Client.GetAsync(client.Url);

        //     TarefasViewModel tarefasViewModel = new TarefasViewModel();
        //     string result;

        //     if (response.IsSuccessStatusCode)
        //     {
        //         result = await response.Content.ReadAsStringAsync();
        //         tarefasViewModel.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
        //         client.Close();

        //         return View(tarefasViewModel);
        //     }

        //     return View("index");
        // }

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            // ToDo - JM
            // Implementar Modal para editar DataLimite
            // ******************

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

        // Ainda não está usando, mas pode usar nos Filtros
        // public async Task<IActionResult> ConsultarTarefasGestorStatus([FromQuery] int idGestor, [FromQuery] int idStatus)
        // {
        //     ApiConnection client = new ApiConnection("usuario/gestor/"+idGestor+"/tarefa/status/"+idStatus);
        //     HttpResponseMessage response = await client.Client.GetAsync(client.Url);
        //     TarefasGestorStatusViewModel tarefasResult = new TarefasGestorStatusViewModel();
        //     string result;
        //     if (response.IsSuccessStatusCode)
        //     {
        //         result = await response.Content.ReadAsStringAsync();
        //         tarefasResult.Tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(result);
        //         client.Close();
        //         return View(tarefasResult);
        //     }
        //     Console.WriteLine(response.StatusCode);  
        //     return View();
        // }

        // ConsultarTarefasUsuario
        public async Task<IActionResult> Listar(int id) /*todas*/
        {
            // ToDo - JM
            // VAI MUDAR O ENDPOINT DA API PARA QUE SIRVA PARA ID USUARIO,
            // INDEPENDENTE DE SER GESTOR OU SUBORDINADO
            // *********
            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/tarefa/pendentes");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();
                Console.WriteLine(response.StatusCode);
                
                return View(tarefas);
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }
//                      
        public async Task<IActionResult> VerTodas(int id) /*todas*/
        {
             // ToDo - JM
            // VAI MUDAR O ENDPOINT DA API PARA QUE SIRVA PARA ID USUARIO,
            // INDEPENDENTE DE SER GESTOR OU SUBORDINADO
            // *********

            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/tarefa/todas");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                client.Close();
                Console.WriteLine(response.StatusCode);
                
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
                // ToDo - JM
                // Por enquanto esta estatico, porem depois que pegarmos
                // O id da session, mudará aqui
                //*************
                return RedirectToAction("Listar", "Tarefa", new { id = 1 });
            }
            return View();
        }

        // public async Task<IActionResult> EditarDataLimite([FromQuery] int idTarefa, EditarDataLimiteViewModel novaData)
        // {
        //     ApiConnection client = new ApiConnection($"tarefa/{idTarefa}/datalimite");
        //     HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, novaData);

        //     if (response.IsSuccessStatusCode)
        //     {
        //         Console.WriteLine("entrou");
        //         client.Close();
        //         Console.WriteLine(novaData.DataLimite.GetType());
        //         // return RedirectToAction("Index", "Home");
        //         return RedirectToAction(nameof(Index));
        //     }
        //     Console.WriteLine("errado");
        //     // client.Close();
        //     return View();
        // }


        public async Task<IActionResult> ConsultarTarefa(int id)
        {
            ApiConnection client = new ApiConnection("tarefa/" + id);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Tarefa tarefa;
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tarefa = JsonConvert.DeserializeObject<Tarefa>(result);
                
                return View(tarefa);
            }
            return View();
        }

    }
}