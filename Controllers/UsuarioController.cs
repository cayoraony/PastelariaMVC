using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;

namespace PastelariaMvc.Controllers
{
    public class UsuarioController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> HomeGestor(int id) // TODO: O nome da action não deixou claro o que faz, está parecendo nome de controller
        {
            GestorHomeViewModel subordinadosResult = new GestorHomeViewModel();
            
            Session session = GetSession();

            if(id != session.idUsuario)
            {
                return RedirectToAction("HomeGestor", "Usuario", new {id = session.idUsuario});
            }

            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/subordinados", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            // : Ver como não encadar um if dentro do outro
            // string result;
            // TODO: Alterar para o enum de status code
            // TODO: Ver porque esses códigos se repetem varias vezes
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return RedirectToAction("Criar", "Usuario");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }
            
            // result = await response.Content.ReadAsStringAsync();
            // subordinadosResult.Subordinados = JsonConvert.DeserializeObject<List<Usuario>>(result);

            subordinadosResult.Subordinados = DeserializeObject<List<Usuario>>(response).Result;
            client.Close();

            ApiConnection clientParaTotal = new ApiConnection($"usuario/{id}/tarefa/total");
            HttpResponseMessage responseParaTotal = await clientParaTotal.Client.GetAsync(clientParaTotal.Url);
            if (responseParaTotal.IsSuccessStatusCode)
            {
                subordinadosResult.QteTotalTarefas = int.Parse(await responseParaTotal.Content.ReadAsStringAsync());

                clientParaTotal.Close();
                return View(subordinadosResult);
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        
        [HttpGet]
        public IActionResult Criar()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromForm] Usuario usuario)
        {
            string stringApi;

            if (usuario.EGestor)
            {
                stringApi = "usuario/gestor/criar";
            }
            else
            {
                stringApi = "usuario/subordinado/criar";
            }
            
            Session session = GetSession();

            usuario.IdGestor = short.Parse(session.idUsuario.ToString());
            usuario.EstaAtivo = true;
            if(usuario.Endereco.Complemento == null)
                usuario.Endereco.Complemento = "";
            ApiConnection client = new ApiConnection(stringApi, session.token);
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("HomeGestor", "Usuario", new {id = session.idUsuario});
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return RedirectToAction("Index", "Error", new { Erro = "Ocorreu um erro com o envio do formulario." });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarUsuario(int id)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            // string result;
            
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return RedirectToAction("Index", "Error", new { Erro = "Usuário não existe, tente outro." });
            }
            else if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }
            // result = await response.Content.ReadAsStringAsync();
            // usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);

            usuarioResult = DeserializeObject<Usuario>(response).Result;
            client.Close();
            if (session.idUsuario == int.Parse(usuarioResult.IdUsuario.ToString()) ||
                session.idUsuario == int.Parse(usuarioResult.IdGestor.ToString()))
            {
                return View(usuarioResult);
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }


        [HttpGet]
        public async Task<IActionResult> AtualizarSubordinado(int id)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            // string result;
            
            if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }
            // result = await response.Content.ReadAsStringAsync();
            // usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);

            usuarioResult = DeserializeObject<Usuario>(response).Result;
            client.Close();

            if(usuarioResult.IdGestor == session.idUsuario || usuarioResult.IdUsuario == session.idUsuario)
            {
                AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
                usuario.IdUsuario = id;

                return View(usuario);
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> SubordinadoPut(int id, AtualizarUsuarioViewModel usuario)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}/atualizar", session.token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarUsuario", "Usuario", new { id = id });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> AtualizarGestor(int id) 
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/{id}", session.token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            // string result;
            
            if (!response.IsSuccessStatusCode)
            {
                return await VerificarErroAsync(response);
            }
            // result = await response.Content.ReadAsStringAsync();
            // usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);

            usuarioResult = DeserializeObject<Usuario>(response).Result;
            client.Close();
            if(usuarioResult.IdUsuario == session.idUsuario)
            {
                AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
                usuario.IdUsuario = id;

                return View(usuario);
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> GestorPut(int id, AtualizarUsuarioViewModel usuario)
        {
            Session session = GetSession();

            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/atualizar", session.token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarUsuario", "Usuario", new { id = id });
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }

        public async Task<IActionResult> AtivarDesativar(int id)
        {
            Session session = GetSession();

            var requestBody = "";
            ApiConnection client = new ApiConnection($"usuario/{id}/status", session.token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("HomeGestor", "Usuario", new {id = session.idUsuario});
            }
            else
            {
                return await VerificarErroAsync(response);
            }
        }
    }
}
