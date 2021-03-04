using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    public class CriarTarefaViewModel
    {
        public Tarefa Tarefa { get; set; }

        public List<Usuario> Subordinados { get; set; }
    }
}