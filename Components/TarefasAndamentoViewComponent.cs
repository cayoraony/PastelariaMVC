using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PastelariaMvc.Infra;

namespace PastelariaMvc.Components
{
    [ViewComponent(Name="TarefasAndamentoViewComponent")]
    public class TarefasAndamentoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(short id)
        {
            // Console.WriteLine(id);
            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/quantidade");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            string result;
            if(response.IsSuccessStatusCode){
                result = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(result);
                return View(short.Parse(result));
            }
            else
            {
                // Console.WriteLine(response.StatusCode);
            }
            return View();
        }
    }
}