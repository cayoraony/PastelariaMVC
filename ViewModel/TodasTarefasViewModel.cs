using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    // TELA 5
    public class TodasTarefasViewModel
    {
        // Analisar que dados é necessário retornar para configurar Model
        // Traz as tarefas de todos status
        public List<Tarefa> TodasTarefas { get; set; }
    }
}