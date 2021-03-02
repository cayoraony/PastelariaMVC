using System;

namespace PastelariaMvc.Models
{
    public class Gestor
    {
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        // public Date DataNascimento { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public bool EstaAtivo { get; set; } = true;
        public int IdEndereco { get; set; }
        public Endereco Endereco { get; set; }
        public short IdEmail { get; set; }
        public Email Email { get; set; }
        public short IdTelefone { get; set; }
        public Telefone Telefone { get; set; }
    }
}