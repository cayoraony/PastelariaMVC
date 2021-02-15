using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.Models
{
    public class Telefone
    {
        public int IdTelefone { get; set; }
        public int Numero { get; set; }
        public byte DDD { get; set; }
        public byte IdTipo { get; set; }
        public TipoTelefone TipoTelefone { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
