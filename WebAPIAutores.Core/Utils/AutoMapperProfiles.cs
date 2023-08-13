using AutoMapper;
using WebAPIAutores.Core.DTO;
using WebAPIAutores.Core.Entities;

namespace WebAPIAutores.Core.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // MAPPERS AUTORES
            CreateMap<AddAutorDto, Autor>();
            CreateMap<Autor, AutorDto>();
            CreateMap<Autor, AutorLibroDto>().ForMember(autorDto => autorDto.Libros, options => options.MapFrom(MapAutorDtoLibros));
            CreateMap<AutorDto, Autor>();

            // MAPPERS LIBROS
            CreateMap<AddLibroDto, Libro>().ForMember(libro => libro.AutorLibro, options => options.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDto>();
            CreateMap<Libro, LibroAutorDto>().ForMember(libro => libro.Autores, options => options.MapFrom(MapLibroDtoAutores));
            CreateMap<UpdateLibroDto, Libro>().ReverseMap();
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


        private List<AutorDto> MapLibroDtoAutores(Libro libro, LibroDto libroDto)
        {
            var result = new List<AutorDto>();
            if (libro.AutorLibro == null) return result;
            foreach (var autorLibro in libro.AutorLibro)
            {
                result.Add(new AutorDto()
                {
                    ID = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre,
                    Edad = autorLibro.Autor.Edad
                });
            }
            return result;
        }

        private List<LibroDto> MapAutorDtoLibros(Autor autor, AutorDto autorDto)
        {
            var result = new List<LibroDto>();
            if (autor.AutorLibro == null) return result;
            foreach (var autorLibro in autor.AutorLibro)
            {
                result.Add(new LibroDto()
                {
                    ID = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }
            return result;
        }
    }
}