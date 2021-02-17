using System.Collections.Generic;
using PastelariaMVC.Models;

namespace PastelariaMVC.ViewModel
{
    public class TarefasGestorStatusViewModel
    {
        public int IdStatusTarefa { get; set; }
        public List<Tarefa> Tarefas { get; set; }
    }
}