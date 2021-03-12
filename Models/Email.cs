using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Email
    {
        public int IdEmail { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Email em formato inválido")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [Display(Name="Email")]
        public string EnderecoEmail { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
