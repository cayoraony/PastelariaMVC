using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdTarefa { get; set; }
        public Tarefa Tarefa { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}