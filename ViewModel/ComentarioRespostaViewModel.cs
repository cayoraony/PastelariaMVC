using System.Collections.Generic;
using PastelariaMvc.Models;

namespace PastelariaMvc.ViewModel
{
    public class ComentarioRespostaViewModel
    {
        public List<Comentario> Comentarios { get; set; }
        public bool Erro { get; set; }
    }
}