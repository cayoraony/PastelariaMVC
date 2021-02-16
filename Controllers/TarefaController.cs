using Microsoft.AspNetCore.Mvc;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PastelariaMvc.Controllers
{
    public class TarefaController : Controller
    {
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            ApiConnection client = new ApiConnection("tarefa/criar");
            HttpResponseMessage response = await client.Client.PostAsJsonAsync(client.Url, tarefa);
            Console.WriteLine(tarefa.IdGestor);
            Console.WriteLine(tarefa.DataLimite);
            if (response.IsSuccessStatusCode)
            {
                client.Close();
                return View("~/Views/Home/Index.cshtml");
            }

            return View("Criar");
        }
    }
}
