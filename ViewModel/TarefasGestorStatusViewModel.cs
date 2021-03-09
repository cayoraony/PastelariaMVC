using PastelariaMvc.Models;
using System.Collections.Generic;

namespace PastelariaMvc.ViewModel
{
    public class TarefasGestorStatusViewModel
    {
        public int IdStatusTarefa { get; set; }
        public List<Tarefa> Tarefas { get; set; }
    }
}