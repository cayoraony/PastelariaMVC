using System.ComponentModel.DataAnnotations;
namespace PastelariaMvc.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string EnderecoEmail { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}