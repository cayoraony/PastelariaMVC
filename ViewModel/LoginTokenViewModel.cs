using PastelariaMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.ViewModel
{
    public class LoginTokenViewModel: Usuario
    {
        public string Token { get; set; }
    }
}
