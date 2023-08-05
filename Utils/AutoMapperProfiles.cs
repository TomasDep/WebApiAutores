using AutoMapper;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // MAPPERS AUTORES
            CreateMap<AddAutorDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<AutorDTO, Autor>();

            // MAPPERS LIBROS
            CreateMap<AddLibroDto, Libro>().ForMember(libro => libro.AutorLibro, options => options.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDto>();

            // MAPPERS Comentario
            CreateMap<AddComentarioDto, Comentario>();
            CreateMap<Comentario, ComentarioDto>();
        }

        private List<AutorLibro> MapAutoresLibros(AddLibroDto libroDto, Libro libro)
        {
            var result = new List<AutorLibro>();
            if (libroDto.AutoresIds == null) return result;
            foreach (var autorId in libroDto.AutoresIds)
            {
                result.Add(new AutorLibro() { AutorId = autorId });
            }
            return result;
        }
    }
}