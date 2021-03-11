using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{

    public class Usuario
    {
        public short IdUsuario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Sobrenome { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Compare("Senha", ErrorMessage = "As senhas não batem")]
        public string ConfirmarSenha { get; set; }
        public bool EstaAtivo { get; set; }
        public int IdEndereco { get; set; }
        public Endereco Endereco { get; set; }
        public short IdEmail { get; set; }
        public Email Email { get; set; }
        public short IdTelefone { get; set; }
        public Telefone Telefone { get; set; }
        public short IdGestor { get; set; }
        public Gestor Gestor { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Gestor")]
        public bool EGestor { get; set; }
        public Usuario()
        {
            this.Email = new Email();
            this.Endereco = new Endereco();
            this.Telefone = new Telefone();
            this.Gestor = new Gestor();
        }       
    }
}