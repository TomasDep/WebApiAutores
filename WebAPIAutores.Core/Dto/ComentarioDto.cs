namespace WebAPIAutores.Core.DTO
{
    public class ComentarioDto
    {
        public int ID { get; set; }
        public string Contenido { get; set; }
    }

    public class AddComentarioDto
    {
        public string Contenido { get; set; }
    }
}