using System.ComponentModel.DataAnnotations;

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
        public int IdUsuario { get; set; }
    }
}

