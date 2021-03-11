using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Endereco
    {
        public int IdEndereco { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Rua { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Numero { get; set; }
        public string Complemento { get; set; } = null;
        [Required(ErrorMessage = "Campo obrigatório")]
        public string CEP { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]

        [Display(Name = "Estado")]
        public string UF { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
