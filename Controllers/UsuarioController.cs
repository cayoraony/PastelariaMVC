using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace PastelariaMvc.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        // public async Task<IActionResult> LoginGestor(Usuario ugestor)
        // {
        //     Console.WriteLine(ugestor);
        //     ApiConnection client = new ApiConnection("usuario/gestor/login");
        //     HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, ugestor);
        //     if (response.IsSuccessStatusCode)
        //     {
        //         client.Close();
        //         return RedirectToAction(nameof(Index));    
        //     }
        //     Console.WriteLine(response.StatusCode);
        //     return View();
        // }

        // Primeiro alteração / Teste
        // Error : Ok
        public async Task<IActionResult> HomeGestor(int id)
        {
            GestorHomeViewModel subordinadosResult = new GestorHomeViewModel();
            try
            {
                string token = HttpContext.Session.GetString("Token");
                ApiConnection client = new ApiConnection($"usuario/gestor/{id}/subordinados", token);
                HttpResponseMessage response = await client.Client.GetAsync(client.Url);

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
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
                }
                else if (response.StatusCode.ToString() == "Forbidden")
                {
                    Console.WriteLine("n é gestor");
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync()});
                }
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
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
            try
            {

                string token = HttpContext.Session.GetString("Token");
                ApiConnection client = new ApiConnection(stringApi, token);
                HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction("Index", "Home");
                }
                else if (response.StatusCode.ToString() == "Unauthorized")
                {
                    return RedirectToAction("Login", "Usuario");
                }
                else if (response.StatusCode.ToString() == "Forbidden")
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

        
   


        public async Task<IActionResult> ConsultarUsuario(int id)
        {
            try
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
                    return View(usuarioResult);
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
        public IActionResult AtualizarSubordinado(int id)
        {
            AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
            usuario.IdUsuario = id;
            return View(usuario);
        }

        //[HttpPut]
        //Por algum motivo não funciona se colocar esse decorator
        public async Task<IActionResult> SubordinadoPut(int id,AtualizarUsuarioViewModel usuario)
        {
            try
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
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
               
            }
        }



        public IActionResult AtualizarGestor(int id)
        {
            AtualizarUsuarioViewModel usuario = new AtualizarUsuarioViewModel();
            usuario.IdUsuario = id;
            return View(usuario);
        }

        public async Task<IActionResult> GestorPut(int id, AtualizarUsuarioViewModel usuario)
        {
            try
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
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            
        }

        public async Task<IActionResult> AtivarDesativar(int id)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                var requestBody = "";
                ApiConnection client = new ApiConnection($"usuario/{id}/status", token);
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> Login(Usuario usuario)
        {

            if(HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ApiConnection client = new ApiConnection("usuario/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            
            if (response.IsSuccessStatusCode)
            {
                string fulljson = await response.Content.ReadAsStringAsync();
                LoginTokenViewModel usuariologado = new LoginTokenViewModel();
                usuariologado = JsonConvert.DeserializeObject<LoginTokenViewModel>(fulljson);
                HttpContext.Session.SetString("Token", usuariologado.Token);

                var handler = new JwtSecurityTokenHandler();
                var tokenTest = handler.ReadToken(usuariologado.Token) as JwtSecurityToken;

                var idUsuario = tokenTest.Claims.ToList()[0].Value;
                Console.WriteLine(idUsuario);

                var token = HttpContext.Session.GetString("Token");
                //Console.WriteLine(token);
                client.Close();
                return View("~/Views/Home/Index.cshtml");
            }
            
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
            // return View("~/Views/Usa/Index.cshtml");
        }


        /*public async Task<IActionResult> LoginSubordinado(Usuario subordinado)
        {
            
            ApiConnection client = new ApiConnection("usuario/subordinado/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, subordinado);
                
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return View("~/Views/Home/Index.cshtml");
            }
            Console.WriteLine(response.StatusCode);
            
            
            return View();
        }*/


    }
}
