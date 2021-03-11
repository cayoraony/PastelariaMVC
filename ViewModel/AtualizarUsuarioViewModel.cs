using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.ViewModel
{
    public class AtualizarUsuarioViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome:")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Sobrenome:")]
        public string Sobrenome { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Compare("Senha", ErrorMessage = "As senhas não batem")]
        public string ConfirmarSenha { get; set; }

        public int IdUsuario { get; set; }
    }
}

