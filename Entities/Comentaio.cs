using Microsoft.AspNetCore.Identity;

namespace WebAPIAutores.Entities
{
    public class Comentario
    {
        public int ID { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public string UsuarioID { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}