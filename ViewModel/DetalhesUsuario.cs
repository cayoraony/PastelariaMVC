using System;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    // TELA 9
    public class DetalhesUsuario
    {
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool EstaAtivo { get; set; }

        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }

        public string EnderecoEmail { get; set; }
        
        public int NumeroTelefone { get; set; }
        public byte DDD { get; set; }
    }
}