namespace PastelariaMVC.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public string Descricao { get; set; }
        public int IdTarefa { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}