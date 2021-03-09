using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    public class GestorHomeViewModel
    {
        public List<Usuario> Subordinados { get; set; }
        public int QteTotalTarefas { get; set; }
    }
}