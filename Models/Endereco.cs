using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Endereco
    {
        public int IdEndereco { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; } = null;
        public string CEP { get; set; }
        public string Cidade { get; set; }

        [Display(Name = "Estado")]
        public string UF { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
