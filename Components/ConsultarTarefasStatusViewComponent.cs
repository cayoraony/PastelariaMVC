using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;

namespace PastelariaMvc.Components
{
    [ViewComponent(Name = "ConsultarTarefasStatusViewComponent")]
    public class ConsultarTarefasStatusViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id, int status)
        {
            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/status/{status}");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            ConsultarTarefasUsuarioViewModel tarefas = new ConsultarTarefasUsuarioViewModel();
            tarefas.Lista = new List<Tarefa>();
            // List<Tarefa> Lista = new List<Tarefa>();
            string result;
            
            if (response.IsSuccessStatusCode)
            {
                
                result = await response.Content.ReadAsStringAsync();
                
                tarefas.Lista = JsonConvert.DeserializeObject<List<Tarefa>>(result);
                //return View(Lista);
            }

            return View(tarefas);
        }
    }
}