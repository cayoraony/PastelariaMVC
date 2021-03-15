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

namespace PastelariaMvc.Controllers
{
    public class UsuarioController : Controller
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

            // TODO: Ver como não encadar um if dentro do outro
            string result;
            if (response.IsSuccessStatusCode)
            {
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
            } 
            // TODO: Alterar para o enum de status code
            // TODO: Ver porque esses códigos se repetem varias vezes
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else if (response.StatusCode.ToString() == "Forbidden")
            {
                    
                return RedirectToAction("Login", "Usuario");
            }
            else if (response.StatusCode.ToString() == "BadRequest")
            {
                return RedirectToAction("Criar", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync()});
            }
            
            return View();        
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else if (response.StatusCode.ToString() == "Forbidden")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else if (response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError")
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
            if (response.IsSuccessStatusCode)
            {
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else if (response.StatusCode.ToString() == "InternalServerError")
            {
                return RedirectToAction("Index", "Error", new { Erro = "Usuário não existe, tente outro." });
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
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
            if (response.IsSuccessStatusCode)
            {
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
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
            if (response.IsSuccessStatusCode)
            {
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            }
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
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
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                return RedirectToAction("Login", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
            } 
        }

        [HttpGet]
        public IActionResult Login()
        {
            // TODO: Remover encademaneto de if
            if(HttpContext.Session.GetString("Token") != null)
            {
                // TODO: Mover variáveis de acesso as informações do usuário para o base controller
                var teste = HttpContext.Session.GetString("Token");
                var idUsuario = DecodeToken.getId(teste);
                var eGestor = DecodeToken.getEGestor(teste);
                if (eGestor)
                {
                    return RedirectToAction("HomeGestor", "Usuario", new { id = idUsuario });
                }
                else if (!eGestor) // TODO: Esse if é inutil
                {
                    return RedirectToAction("Listar", "Tarefa", new { id = idUsuario });
                }
            }
            return View();
        }

        // TODO: Melhorar esse metodo como um todo
        public async Task<IActionResult> LoginPost(Usuario usuario)
        {
            // TODO: Ver como não replicar esse código
            if(HttpContext.Session.GetString("Token") != null)
            {
                var teste = HttpContext.Session.GetString("Token");
                var idUsuario = DecodeToken.getId(teste);
                var eGestor = DecodeToken.getEGestor(teste);
                if (eGestor)
                {
                    return RedirectToAction("HomeGestor", "Usuario", new { id = idUsuario });
                }
                else if (!eGestor)
                {
                    return RedirectToAction("Listar", "Tarefa", new { id = idUsuario });
                }
            }

            ApiConnection client = new ApiConnection("usuario/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                // TODO: Criar um método genérico pra fazer a leitura
                string fulljson = await response.Content.ReadAsStringAsync();
                LoginTokenViewModel usuariologado = new LoginTokenViewModel();
                usuariologado = JsonConvert.DeserializeObject<LoginTokenViewModel>(fulljson);

                // TODO: Ver como pegar essas variaveis em outro lugar
                HttpContext.Session.SetString("Token", usuariologado.Token);
                var teste = HttpContext.Session.GetString("Token");
                var idUsuario = DecodeToken.getId(teste);
                var eGestor = DecodeToken.getEGestor(teste);
                var token = HttpContext.Session.GetString("Token");
                client.Close();

                // TODO: GZUIS 
                if(eGestor)
                {
                    return RedirectToAction("HomeGestor", "Usuario", new {id = idUsuario});
                }
                else if(!eGestor)
                {
                    return RedirectToAction("Listar", "Tarefa", new {id = idUsuario});
                }
                else
                {
                    return RedirectToAction("Error", "Index");
                }   
            }
            // ToDo - JM
            // Fazer alguma notificação ou encaminhar para página que mostre senha errada
            return RedirectToAction("Login", "Usuario");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }
    }
}
