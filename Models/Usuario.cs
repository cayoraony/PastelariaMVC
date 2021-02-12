using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastelariaMvc.Models
{
    
    public class Usuario
    {
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public bool EstaAtivo { get; set; } = true;
        public int IdEndereco { get; set; }
        //public Endereco Endereco { get; set; }
        public short IdEmail { get; set; }
        //public Email Email { get; set; }
        public short IdTelefone { get; set; }
        //public Telefone Telefone { get; set; }

        /*
        public Usuario()
        {
            this.Email = new Email();
            this.Endereco = new Endereco();
            this.Telefone = new Telefone();
        }
        */

        //public List<Tarefa> Tarefas { get; set; }

        public Usuario()
        {

        }
            
    }
}