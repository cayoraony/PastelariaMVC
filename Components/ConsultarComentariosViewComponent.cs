using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PastelariaMvc.Infra;
using PastelariaMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PastelariaMvc.Components
{
    [ViewComponent(Name = "ConsultarComentariosViewComponent")]
    public class ConsultarComentariosViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            ApiConnection client = new ApiConnection("tarefa/" + id + "/comentarios");
            HttpResponseMessage response = await client.Client.GetAsync(client.Url);
            List<Comentario> comentarios;
            string result;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                comentarios = JsonConvert.DeserializeObject<List<Comentario>>(result);
                return View(comentarios);
            }

            return View();
        }
    }
}
