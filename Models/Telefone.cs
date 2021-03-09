using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Telefone
    {
        public int IdTelefone { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }
        public byte DDD { get; set; }

        [Display(Name = "Tipo de Telefone")]
        public byte IdTipo { get; set; }
        public TipoTelefone TipoTelefone { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
