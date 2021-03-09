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
            ApiConnection client = new ApiConnection($"usuario/{id}/tarefa/total");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            string result;
            if(response.IsSuccessStatusCode){
                result = await response.Content.ReadAsStringAsync();
                return View(short.Parse(result));
            }
            return View();
        }
    }
}