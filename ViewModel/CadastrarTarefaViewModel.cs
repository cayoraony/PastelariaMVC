using System;
using System.ComponentModel.DataAnnotations;
using PastelariaMvc.Models;

namespace PastelariaMVC.ViewModel
{
    // TELA 4
    public class CadastrarTarefa
    {
        [Required]
        public string Descricao { get; set; }
        public DateTime DataLimite { get; set; }
        // id e nome
        public Usuario Subordinado { get; set; }
    }
}