using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
            var url = "http://localhost:5000/api/usuario/gestor/"+id.ToString();
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(url);
            Usuario usuarioResult;
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                usuarioResult = JsonConvert.DeserializeObject<Usuario>(result);
                return View(usuarioResult);
            }
            else
            {
                result = "Deu merda";
            }
            client.Dispose();
            return View();
        }

        public async Task<IActionResult> PostUser(Usuario usuario)
        {
            var url = "http://localhost:5000/api/usuario/gestor/criar";
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsJsonAsync(url, usuario);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Deu certo aaaaa");
            }
            return View();

        }
    }
}
