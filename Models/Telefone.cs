using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Telefone
    {
        public int IdTelefone { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Número")]
        [Range(80000000, 999999999, ErrorMessage = "Preencha com até 9 digítos")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(10, 99, ErrorMessage = "Preencha com até 2 digítos")]
        public byte DDD { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Tipo de Telefone")]
        public byte IdTipo { get; set; }
        public TipoTelefone TipoTelefone { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
