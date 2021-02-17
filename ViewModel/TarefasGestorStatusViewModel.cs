using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    public class TarefasGestorStatusViewModel
    {
        public int IdStatusTarefa { get; set; }
        public List<Tarefa> Tarefas { get; set; }
    }
}