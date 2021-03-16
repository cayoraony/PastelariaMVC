using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class TarefaController : BaseController
    {
        public async Task<IActionResult> Cancelar(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var requestBody = "";
            ApiConnection client = new ApiConnection($"tarefa/{id}/cancelar", token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            bool eGestorLogado = DecodeToken.getEGestor(token);
            CriarTarefaViewModel criarTarefa = new CriarTarefaViewModel();

            if(!eGestorLogado)
            {
                return View(criarTarefa);
            }

            ApiConnection client = new ApiConnection($"usuario/gestor/{idLogado}/subordinados", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            // string result;
            if (response.IsSuccessStatusCode)
            {
                // result = await response.Content.ReadAsStringAsync();
                // criarTarefa.Subordinados = JsonConvert.DeserializeObject<List<Usuario>>(result);

                criarTarefa.Subordinados = DeserializeObject<List<Usuario>>(response).Result;
                client.Close();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return RedirectToAction("Index", "Error", new { Erro = "Ocorreu um erro com o envio do formulario." });
    
            }
            return View(criarTarefa);
        }
        
        public async Task<IActionResult> CriarTarefa(Tarefa tarefa)
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
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            string token = HttpContext.Session.GetString("Token");
            ApiConnection client = new ApiConnection($"tarefa/{id}/datalimite", token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);

            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }  
        }

        public async Task<IActionResult> CriarComentario(int id, Comentario comentario)
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
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);

            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/andamento", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                
            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            // string result;
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine(response.StatusCode.ToString());
                return RedirectToAction("Criar", "Tarefa");
            }
            
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }

            // result = await response.Content.ReadAsStringAsync();
            // tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);

            tarefas.Lista = DeserializeObject<List<Tarefa>>(response).Result;

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
        
        [HttpGet]
        public async Task<IActionResult> VerTodas(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);

            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/todas", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            string result;
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine(response.StatusCode.ToString());
                return RedirectToAction("Criar", "Tarefa");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }

            // result = await response.Content.ReadAsStringAsync();
            // tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);

            tarefas.Lista = DeserializeObject<List<Tarefa>>(response).Result;
                
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

        public async Task<IActionResult> Concluir(int id)
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
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarTarefa(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            ApiConnection client = new ApiConnection("tarefa/" + id, token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Comentario comentario = new Comentario();
            // string result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
            
            // result = await response.Content.ReadAsStringAsync();
            // comentario.Tarefa = JsonConvert.DeserializeObject<Tarefa>(result);

            comentario.Tarefa = DeserializeObject<Tarefa>(response).Result;

            if (comentario.Tarefa.IdGestor == idLogado || comentario.Tarefa.IdSubordinado == idLogado)
            {
                return View(comentario);
            }
            return RedirectToAction("Index", "Error", new { Erro = "Você não tem acesso a esta página" });
            
        }
    }
}