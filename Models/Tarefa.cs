using System;
using System.ComponentModel.DataAnnotations;

namespace PastelariaMvc.Models
{
    public class Tarefa
    {
        public int IdTarefa { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data Limite")]
        public DateTime DataLimite { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; } = null;
        public DateTime? DataCancelada { get; set; } = null;

        public int IdGestor { get; set; }
        public Usuario Gestor { get; set; }


        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Subordinado")]
        public int IdSubordinado{ get; set; }
        public Usuario Subordinado { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Status")]
        public int IdStatusTarefa { get; set; }
        public StatusTarefa Status { get; set; }

        public Tarefa()
        {
            this.Status = new StatusTarefa();
        }
    }
}