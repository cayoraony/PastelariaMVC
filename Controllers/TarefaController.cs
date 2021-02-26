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

        public async Task<IActionResult> Cancelar(int id)
        {
            try
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
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }

        
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            try
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

                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }


        

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            // ToDo - JM
            // Implementar Modal para editar DataLimite
            // ******************
            try
            {

                ApiConnection client = new ApiConnection($"tarefa/{id}/datalimite");
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }

        public async Task<IActionResult> CriarComentario(int id, Comentario comentario)
        {
            try
            {

                ApiConnection client = new ApiConnection($"tarefa/{id}/comentario/criar");
                HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, comentario);
                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }


        // ConsultarTarefasUsuario
        public async Task<IActionResult> Listar(int id) /*todas*/
        {
            // ToDo - JM (OK)
            // VAI MUDAR O ENDPOINT DA API PARA QUE SIRVA PARA ID USUARIO,
            // INDEPENDENTE DE SER GESTOR OU SUBORDINADO
            // *********
            try
            {

                ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/andamento");
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
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            return View();
        }
        

        public async Task<IActionResult> VerTodas(int id) /*todas*/
        {
            // ToDo - JM (OK)
            // VAI MUDAR O ENDPOINT DA API PARA QUE SIRVA PARA ID USUARIO,
            // INDEPENDENTE DE SER GESTOR OU SUBORDINADO
            // *********
            try
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
                    Console.WriteLine(response.StatusCode);
                
                    return View(tarefas);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }

        public async Task<IActionResult> Concluir(int id)
        {
            try
            {
                var requestBody = "";
                ApiConnection client = new ApiConnection($"tarefa/{id}/concluir");
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
           
                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    // ToDo - JM
                    // Por enquanto esta estatico, porem depois que pegarmos
                    // O id da session, mudará aqui
                    //*************
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }

                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
            return View();
        }

        public async Task<IActionResult> ConsultarTarefa(int id)
        {
            try
            {

                ApiConnection client = new ApiConnection("tarefa/" + id);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                // Tarefa tarefa;
                Comentario comentario = new Comentario();
                string result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    comentario.Tarefa = JsonConvert.DeserializeObject<Tarefa>(result);
                
                    return View(comentario);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
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
    }
}