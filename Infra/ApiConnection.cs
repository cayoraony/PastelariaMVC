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

        public ApiConnection(string endUrl)
        {
            this.Url = "http://localhost:5000/api/" + endUrl ;
            this.Client = new HttpClient();
            this.Client.BaseAddress = new Uri(this.Url);
        }
        
        
        public void Close()
        {
            this.Client.Dispose();
        }
      
    }
}
