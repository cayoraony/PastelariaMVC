using System;
using PastelariaMvc.Models;

namespace PastelariaMVC.ViewModel
{
    // TELA 6
    public class DetalhesTarefaViewModel
    {
        public string Descricao { get; set; }
        public DateTime DataLimite { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataCancelada { get; set; }
        public Usuario Gestor { get; set; }
        public Usuario Subordinado { get; set; }

        // Usuario deve retornar => nome, sobrenome, idUsuario
    }
}