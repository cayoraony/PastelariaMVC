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
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> LoginGestor(Usuario ugestor)
        {
            Console.WriteLine(ugestor);
            ApiConnection client = new ApiConnection("usuario/gestor/login");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, ugestor);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction(nameof(Index));    
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }

        // Primeiro alteração / Teste
        // Error : Ok
        public async Task<IActionResult> HomeGestor(int id)
        {
            GestorHomeViewModel subordinadosResult = new GestorHomeViewModel();
            try
            {
                ApiConnection client = new ApiConnection($"usuario/gestor/{id}/subordinados");
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

        public async Task<IActionResult> Criar([FromForm] Usuario usuario)
        {
            string stringApi;

            if(usuario.EGestor) 
            {
                stringApi = "usuario/gestor/criar";
            }
            else 
            {
                stringApi = "usuario/subordinado/criar";
            }
            try
            {
                //Gambiarrazada :)
                if(usuario.Nome != "")
                {
                    ApiConnection client = new ApiConnection(stringApi);
                    HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
                
                    if (response.IsSuccessStatusCode)
                    {
                        client.Close();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { Erro = await response.Content.ReadAsStringAsync() });
                    }
                } 
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", "Error", new { Erro = exception.Message.ToString() });
            }
            return View();
        }
   

        public async Task<IActionResult> ConsultarUsuario(int id)
        {
            try
            {
                ApiConnection client = new ApiConnection($"usuario/{id}");
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



        public async Task<IActionResult> AtualizarSubordinado(int id, AtualizarUsuarioViewModel usuario)
        {
            try
            {

                ApiConnection client = new ApiConnection($"usuario/{id}/atualizar");
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction(nameof(Index));
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



        public async Task<IActionResult> AtualizarGestor(int id, AtualizarUsuarioViewModel usuario)
        {
            try
            {
                ApiConnection client = new ApiConnection($"usuario/gestor/{id}/atualizar");
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, usuario);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> AtivarDesativar(int id)
        {
            try
            {
                var requestBody = "";
                ApiConnection client = new ApiConnection($"usuario/{id}/status");
                HttpResponseMessage response = await client.Client.PutAsJsonAsync(client.Url, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    client.Close();
                    return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> LoginSubordinado(Usuario subordinado)
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
        }


    }
}
