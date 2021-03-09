using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Email
    {
        public int IdEmail { get; set; }

        [Display(Name="Email")]
        public string EnderecoEmail { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
