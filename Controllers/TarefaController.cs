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
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            Session session = GetSession();
            CriarTarefaViewModel criarTarefa = new CriarTarefaViewModel();

            if(!session.eGestor)
            {
                return View(criarTarefa);
            }

            ApiConnection client = new ApiConnection($"usuario/gestor/{session.idUsuario}/subordinados", session.token);
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
            Session session = GetSession();
            // string token = HttpContext.Session.GetString("Token");
            // int idLogado = DecodeToken.getId(token);
            tarefa.IdGestor = short.Parse(session.idUsuario.ToString());

            if(!session.eGestor)
            {
                tarefa.IdSubordinado = short.Parse(session.idUsuario.ToString());
            }

            ApiConnection client = new ApiConnection("tarefa/criar", session.token);
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, tarefa);

            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("Listar", "Tarefa", new { id = tarefa.IdGestor });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> EditarDataLimite(int id, EditarDataLimiteViewModel tarefa)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"tarefa/{id}/datalimite", session.token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, tarefa);

            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> CriarComentario(int id, Comentario comentario)
        {
            Session session = GetSession();
            comentario.IdUsuario = session.idUsuario;
            ApiConnection client = new ApiConnection($"tarefa/{id}/comentario/criar", session.token);
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, comentario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/andamento", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
                
            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine(response.StatusCode.ToString());
                return RedirectToAction("Criar", "Tarefa");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }

            // result = await response.Content.ReadAsStringAsync();
            // tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);

            tarefas.Lista = DeserializeObject<List<Tarefa>>(response).Result;

            foreach (var tarefa in tarefas.Lista)
            {
                if(tarefa.IdGestor == session.idUsuario || tarefa.IdSubordinado == session.idUsuario)
                {
                    client.Close();

                    return View(tarefas);
                }
            }

            return RedirectToAction("Listar", "Tarefa", new {id = session.idUsuario});

        }
        
        [HttpGet]
        public async Task<IActionResult> VerTodas(int id)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/todas", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine(response.StatusCode.ToString());
                return RedirectToAction("Criar", "Tarefa");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }

            // result = await response.Content.ReadAsStringAsync();
            // tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);

            tarefas.Lista = DeserializeObject<List<Tarefa>>(response).Result;
                
            foreach (var tarefa in tarefas.Lista)
            {
                if(tarefa.IdGestor == session.idUsuario || tarefa.IdSubordinado == session.idUsuario)
                {
                    client.Close();
                
                    return View(tarefas);
                }
            }

            return RedirectToAction("Listar", "Tarefa", new {id = session.idUsuario});
        }

        public async Task<IActionResult> Concluir(int id)
        {
            Session session = GetSession();

            var requestBody = "";
            ApiConnection client = new ApiConnection($"tarefa/{id}/concluir", session.token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
           
            if (response.IsSuccessStatusCode)
            {
                client.Close();

                return RedirectToAction("ConsultarTarefa", "Tarefa", new { id = id });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarTarefa(int id)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection("tarefa/" + id, session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Comentario comentario = new Comentario();
            
            if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }
            
            // result = await response.Content.ReadAsStringAsync();
            // comentario.Tarefa = JsonConvert.DeserializeObject<Tarefa>(result);

            comentario.Tarefa = DeserializeObject<Tarefa>(response).Result;

            if (comentario.Tarefa.IdGestor == session.idUsuario || comentario.Tarefa.IdSubordinado == session.idUsuario)
            {
                return View(comentario);
            }
            return RedirectToAction("Index", "Error", new { Erro = "Você não tem acesso a esta página" });
        }
    }
}