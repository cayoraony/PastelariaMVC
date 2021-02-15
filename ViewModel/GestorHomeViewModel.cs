using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMVC.ViewModel
{
    //TELA 2
    public class GestorHomeViewModel
    {
        // Busca nome e quantidade total de tarefas
        // na proc pega isso com idUsuario
        public List<Usuario> Subordinados {get; set;}
        // quantidade total do gestor
        public int QteTotalTarefas { get; set; }
    }
}