using System.Collections.Generic;
using PastelariaMVC.Models;

namespace PastelariaMVC.ViewModel
{
    // TELA 8
    public class ComentariosViewModel
    {
        // public int IdTarefa { get; set; }
        public List<Comentario> Comentarios{ get; set; }
        public string Descricao { get; set; }
    }
}