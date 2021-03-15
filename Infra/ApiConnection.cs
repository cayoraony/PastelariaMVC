using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PastelariaMvc.Infra
{
    public class ApiConnection
    {
        public string Url { get; set; }
        public HttpClient Client { get; set; }

        public ApiConnection(string endUrl, string token)
        {
            // TODO: Mover a URL da api para um arquivo de configuração
            this.Url = "http://localhost:5000/api/" + endUrl ;
            this.Client = new HttpClient();
            this.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            this.Client.BaseAddress = new Uri(this.Url);
        }

        public ApiConnection(string endUrl)
        {
            this.Url = "http://localhost:5000/api/" + endUrl;
            this.Client = new HttpClient();
            
            this.Client.BaseAddress = new Uri(this.Url);
        }


        public void Close()
        {
            this.Client.Dispose();
        }
      
    }
}
