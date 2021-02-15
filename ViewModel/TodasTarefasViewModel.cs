using System.Collections.Generic;
using PastelariaMVC.Models;

namespace PastelariaMVC.ViewModel
{
    // TELA 5
    public class TodasTarefasViewModel
    {
        // Analisar que dados é necessário retornar para configurar Model
        // Traz as tarefas de todos status
        public List<Tarefa> TodasTarefas { get; set; }
    }
}