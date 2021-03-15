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
            
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);

            if(id != idLogado)
            {
                return RedirectToAction("HomeGestor", "Usuario", new {id = idLogado});
            }

            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/subordinados", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            // : Ver como não encadar um if dentro do outro
            string result;
            // TODO: Alterar para o enum de status code
            // TODO: Ver porque esses códigos se repetem varias vezes
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return RedirectToAction("Criar", "Usuario");
            }
            
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync()});
            }
            
            result = await response.Content.ReadAsStringAsync();
            subordinadosResult.Subordinados = JsonConvert.DeserializeObject<List<Usuario>>(result);
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
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync()});
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
            
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            usuario.IdGestor = short.Parse(idLogado.ToString());
            usuario.EstaAtivo = true;
            if(usuario.Endereco.Complemento == null)
                usuario.Endereco.Complemento = "";
            ApiConnection client = new ApiConnection(stringApi, token);
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("HomeGestor", "Usuario", new {id = idLogado});
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return RedirectToAction("Index", "Error", new { Erro = "Ocorreu um erro com o envio do formulario." });
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarUsuario(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            ApiConnection client = new ApiConnection($"usuario/{id}", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            string result;
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return RedirectToAction("Index", "Error", new { Erro = "Usuário não existe, tente outro." });
            }
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
            result = await response.Content.ReadAsStringAsync();
            usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);
            client.Close();
            if (DecodeToken.getId(token) == int.Parse(usuarioResult.IdUsuario.ToString()) ||
                DecodeToken.getId(token) == int.Parse(usuarioResult.IdGestor.ToString()))
            {
                return View(usuarioResult);
            }
            return RedirectToAction("Index", "Error", new { Erro = "Você não pode acessar este usuário" });
        }


        [HttpGet]
        public async Task<IActionResult> AtualizarSubordinado(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            ApiConnection client = new ApiConnection($"usuario/{id}", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            string result;
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
            result = await response.Content.ReadAsStringAsync();
            usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);
            client.Close();

            if(usuarioResult.IdGestor == idLogado || usuarioResult.IdUsuario == idLogado)
            {
                AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
                usuario.IdUsuario = id;

                return View(usuario);
            }
            return RedirectToAction("Index", "Error", new { Erro = "Você não pode editar este usuário" });
        }

        public async Task<IActionResult> SubordinadoPut(int id, AtualizarUsuarioViewModel usuario)
        {
            string token = HttpContext.Session.GetString("Token");
            ApiConnection client = new ApiConnection($"usuario/{id}/atualizar", token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarUsuario", "Usuario", new { id = id });
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

        public async Task<IActionResult> AtualizarGestor(int id) 
        { 
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            ApiConnection client = new ApiConnection($"usuario/{id}", token);
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            Usuario usuarioResult;
            string result;
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
            result = await response.Content.ReadAsStringAsync();
            usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);
            client.Close();
            if(usuarioResult.IdUsuario == idLogado)
            {
                AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
                usuario.IdUsuario = id;

                return View(usuario);
            }
            return RedirectToAction("Index", "Error", new { Erro = "Você não pode editar este usuário" });
        }

        public async Task<IActionResult> GestorPut(int id, AtualizarUsuarioViewModel usuario)
        {
            string token = HttpContext.Session.GetString("Token");
            ApiConnection client = new ApiConnection($"usuario/gestor/{id}/atualizar", token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("ConsultarUsuario", "Usuario", new { id = id });
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

        public async Task<IActionResult> AtivarDesativar(int id)
        { 
            string token = HttpContext.Session.GetString("Token");
            int idLogado = DecodeToken.getId(token);
            var requestBody = "";
            ApiConnection client = new ApiConnection($"usuario/{id}/status", token);
            HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction("HomeGestor", "Usuario", new {id = idLogado});
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
    }
}
