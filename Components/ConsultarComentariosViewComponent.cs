using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using PastelariaMvc.ViewModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PastelariaMvc.Components
{
  [ViewComponent(Name = "ConsultarComentariosViewComponent")]
    public class ConsultarComentariosViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
           ApiConnection client = new ApiConnection($"tarefa/{id}/comentarios");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);

            ComentarioRespostaViewModel resposta = new ComentarioRespostaViewModel();
            resposta.Comentarios = new List<Comentario>();
            // List<Comentario> comentarios = new List<Comentario>();
            string result;
            
            if (response.IsSuccessStatusCode)
            {
                
                result = await response.Content.ReadAsStringAsync();
                
                resposta.Comentarios = JsonConvert.DeserializeObject<List<Comentario>>(result);
                //return View(comentarios);
            }

            return View(resposta);
        }
    }
}
