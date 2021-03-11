using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.ViewModel
{
    public class AtualizarUsuarioViewModel
    {
        [Required(ErrorMessage = "Campo obrigat�rio")]
        [Display(Name = "Nome:")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigat�rio")]
        [Display(Name = "Sobrenome:")]
        public string Sobrenome { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigat�rio")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigat�rio")]
        [Compare("Senha", ErrorMessage = "As senhas n�o batem")]
        public string ConfirmarSenha { get; set; }

        public int IdUsuario { get; set; }
    }
}

