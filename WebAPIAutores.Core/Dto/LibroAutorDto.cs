namespace WebAPIAutores.Core.DTO
{
    public class LibroAutorDto : LibroDto
    {
        public List<AutorDto> Autores { get; set; }
    }
}