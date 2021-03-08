using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;

namespace PastelariaMvc.Controllers
{
    public class TarefaController : Controller
    {

        // ToDO - jm
        // Concluir/Cancelar será permitido apenas pelo Gestor/Subordinado da Tarefa?
        // Se sim, retirar os botões de ListarTarefaAnadamento
        // Gestor do subordinado cuja tarefa é o gestor pode Concluir/Cancelar?
        // Se sim, deixar os botões de ListarTarefaAnadamento
        public async Task<IActionResult> Cancelar(int id)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var requestBody = "";
                ApiConnection client = new ApiConnection($"tarefa/{id}/cancelar", token);
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
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
            
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {  
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            bool eGestorLogado = DecodeToken.getEGestor(token);
            CriarTarefaViewModel criarTarefa = new CriarTarefaViewModel();

            if(eGestorLogado)
            {
                ApiConnection client = new ApiConnection($"usuario/gestor/{idLogado}/subordinados", token);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                string result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    criarTarefa.Subordinados = JsonConvert.DeserializeObject<List<Usuario>>(result);
                    client.Close();
                }

                return View(criarTarefa);
            }

            return View(criarTarefa);
        }
        
        public async Task<IActionResult> CriarTarefa(Tarefa tarefa)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                int idLogado = DecodeToken.getId(token);
                tarefa.IdGestor = short.Parse(idLogado.ToString());

                if(!DecodeToken.getEGestor(token))
                {
                    tarefa.IdSubordinado = short.Parse(idLogado.ToString());
                }

                ApiConnection client = new ApiConnection("tarefa/criar", token);
                HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, tarefa);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("Listar", "Tarefa", new { id = tarefa.IdGestor });
                }
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
            
        }


        

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                ApiConnection client = new ApiConnection($"tarefa/{id}/datalimite", token);
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
            
        }

        public async Task<IActionResult> CriarComentario(int id, Comentario comentario)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                comentario.IdUsuario = DecodeToken.getId(token);
                ApiConnection client = new ApiConnection($"tarefa/{id}/comentario/criar", token);
                HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, comentario);
                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
            
        }


    
        // ConsultarTarefasUsuario
        [HttpGet]
        public async Task<IActionResult> Listar(int id) /*todas*/
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                int idLogado = DecodeToken.getId(token);

                ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/andamento", token);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                
                ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
                string result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);

                    foreach (var tarefa in tarefas.Lista)
                    {
                        if(tarefa.IdGestor == idLogado || tarefa.IdSubordinado == idLogado)
                        {
                            client.Close();
                    
                            return View(tarefas);
                        }
                    }

                    return RedirectToAction("Listar", "Tarefa", new {id = idLogado});
                }

                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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

        }
        
        [HttpGet]
        public async Task<IActionResult> VerTodas(int id) /*todas*/
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                int idLogado = DecodeToken.getId(token);

                ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/todas", token);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);

                ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
                string result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                    
                    foreach (var tarefa in tarefas.Lista)
                    {
                        if(tarefa.IdGestor == idLogado || tarefa.IdSubordinado == idLogado)
                        {
                            client.Close();
                    
                            return View(tarefas);
                        }
                    }

                    return RedirectToAction("Listar", "Tarefa", new {id = idLogado});
                }

                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
            
        }

        // ToDO - jm
        // Concluir/Cancelar será permitido apenas pelo Gestor/Subordinado da Tarefa?
        // Se sim, retirar os botões de ListarTarefaAnadamento
        // Gestor do subordinado cuja tarefa é o gestor pode Concluir/Cancelar?
        // Se sim, deixar os botões de ListarTarefaAnadamento
        public async Task<IActionResult> Concluir(int id)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var requestBody = "";
                ApiConnection client = new ApiConnection($"tarefa/{id}/concluir", token);
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
           
                if (response.IsSuccessStatusCode)
                {
                    client.Close();

                    return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
                }
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarTarefa(int id)
        {
            if(HttpContext.Session.GetString("Token") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            try
            {
                string token = HttpContext.Session.GetString("Token");
                int idLogado = DecodeToken.getId(token);

                ApiConnection client = new ApiConnection("tarefa/" + id, token);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                // Tarefa tarefa;
                Comentario comentario = new Comentario();
                string result;
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    comentario.Tarefa = JsonConvert.DeserializeObject<Tarefa>(result);
                
                    if(comentario.Tarefa.IdGestor == idLogado || comentario.Tarefa.IdSubordinado == idLogado)
                    {
                            return View(comentario);
                    }
                    
                    return RedirectToAction("Index", "Error", new { Erro = "Você não tem acesso a esta página" });
                }
                
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
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
        //     return View();
        // }

        
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
        //     return View();
        // }
    }
}