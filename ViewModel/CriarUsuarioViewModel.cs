using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMVC.ViewModel
{
    // TELA 10
    public class CriarUsuarioViewModel
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sobrenome { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public bool EstaAtivo { get; set; }

        [Required]
        public string Rua { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Complemento { get; set; }
        [Required]
        public string CEP { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string UF { get; set; }

        [Required]
        public string EnderecoEmail { get; set; }
        
        [Required]
        public int NumeroTelefone { get; set; }
        [Required]
        public byte DDD { get; set; }
    }
}