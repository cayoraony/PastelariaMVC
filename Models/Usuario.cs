using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PastelariaMvc.Models;

namespace PastelariaMvc.Models
{
    
    public class Usuario
    {
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        // public Date DataNascimento { get; set; }
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }

        public bool EstaAtivo { get; set; }

        public int IdEndereco { get; set; }
        public Endereco Endereco { get; set; }
        public short IdEmail { get; set; }
        public Email Email { get; set; }
        public short IdTelefone { get; set; }
        public Telefone Telefone { get; set; }
        public short IdGestor { get; set; }
        public Gestor Gestor { get; set; }

        [Display(Name = "Gestor")]
        public bool EGestor { get; set; }

        public Usuario() {
            this.Email = new Email();
            this.Endereco = new Endereco();
            this.Telefone = new Telefone();
            this.Gestor = new Gestor();
        }   
        //public List<Tarefa> Tarefas { get; set; }        
    }
}