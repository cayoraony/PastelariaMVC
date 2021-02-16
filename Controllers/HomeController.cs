using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Exemplo
        public async Task<IActionResult> GetUser(int id)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/"+id);
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
            Console.WriteLine(response.StatusCode);  
            return View();
        }

        public async Task<IActionResult> PostUser(Usuario usuario)
        {
            ApiConnection client = new ApiConnection("usuario/gestor/criar");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, usuario);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return RedirectToAction(nameof(Index));    
            }
            Console.WriteLine(response.StatusCode);
            return View();
        }
    }
}
