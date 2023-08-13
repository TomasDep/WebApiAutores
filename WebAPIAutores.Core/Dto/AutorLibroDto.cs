namespace WebAPIAutores.Core.DTO
{
    public class AutorLibroDto : AutorDto
    {
        public List<LibroDto> Libros { get; set; }
    }
}