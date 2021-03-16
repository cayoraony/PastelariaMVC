namespace PastelariaMvc.Models
{
    public class Session
    {
        public string token { get; set; }
        public int idUsuario { get; set; }
        public bool eGestor { get; set; }
        public string nomeUsuario { get; set; }

    }
}