using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.ViewModel
{
    public class AtualizarUsuarioViewModel
    {
        [Display(Name = "Nome:")]
        public string Nome { get; set; }
        [Display(Name = "Sobrenome:")]
        public string Sobrenome { get; set; }
        [Display(Name = "Senha:")]
        public string Senha { get; set; }
    }
}

