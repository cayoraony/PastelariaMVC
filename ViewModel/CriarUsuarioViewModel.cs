using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.ViewModel
{
    // TELA 10
    public class CriarUsuarioViewModel
    {
        
        public string Nome { get; set; }
        
        public string Sobrenome { get; set; }
        
        public DateTime DataNascimento { get; set; }
        
        public string Senha { get; set; }
        
        public bool EGestor { get; set; } = true;
        
        public bool EstaAtivo { get; set; } = true;
        
        public short IdGestor { get; set; }
        
        
        public string EnderecoEmail { get; set; }

        
        public byte DDD { get; set; }
        
        public int Telefone { get; set; }
        
        public byte IdTipoTelefone { get; set; }

        
        public string Rua { get; set; }
        
        public string Bairro { get; set; }
        
        public string Numero { get; set; }
        
        public string Complemento { get; set; }
        
        public string CEP { get; set; }
        
        public string Cidade { get; set; }
        
        public string UF { get; set; }
    }
}