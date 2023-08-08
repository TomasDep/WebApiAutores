using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Services
{
    public class LibrosServicesImpl : ILibrosServices
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosServicesImpl(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IActionResult> CreateLibro(AddLibroDto addlibroDto, ILogger log)
        {
            try
            {
                if (addlibroDto.AutoresIds == null)
                {
                    return new BadRequestObjectResult("You can't create a book without authors") { StatusCode = 400 };
                }
                var autoresIds = await context.Autores.Where(autorDB => addlibroDto.AutoresIds.Contains(autorDB.ID)).Select(autor => autor.ID).ToListAsync();
                if (addlibroDto.AutoresIds.Count != autoresIds.Count)
                {
                    return new BadRequestObjectResult("Not existing authors") { StatusCode = 400 };

                }
                var newLibro = mapper.Map<Libro>(addlibroDto);
                AssignOrderAuthors(newLibro);
                context.Add(newLibro);
                await context.SaveChangesAsync();
                var libroDto = mapper.Map<LibroDto>(newLibro);
                log.LogInformation("Finish Post");
                return new ObjectResult(libroDto) { StatusCode = 201, Value = libroDto };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult<LibroAutorDto>> GetLibroById(int id, ILogger log)
        {
            try
            {
                var librosDB = await context.Libros.Include(lib => lib.AutorLibro)
                    .ThenInclude(autorLibroBD => autorLibroBD.Autor)
                    .FirstOrDefaultAsync(libro => libro.ID == id);
                if (librosDB == null)
                {
                    return new NotFoundObjectResult($"Book with ID: {id} not exist") { StatusCode = 404 };

                }
                librosDB.AutorLibro = librosDB.AutorLibro.OrderBy(autlib => autlib.Orden).ToList();
                var libros = mapper.Map<LibroAutorDto>(librosDB);
                log.LogInformation("Finish Get");
                return new ObjectResult(libros) { StatusCode = 200, Value = libros };

            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> RemoveLibro(int id, ILogger log)
        {
            try
            {
                var existLibro = await context.Libros.AnyAsync(libro => libro.ID == id);
                if (!existLibro)
                {
                    return new NotFoundObjectResult($"Book with ID: {id} not exist") { StatusCode = 404 };
                }
                context.Remove(new Libro() { ID = id });
                await context.SaveChangesAsync();
                return new ObjectResult("") { StatusCode = 204 };

            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> UpdateLibro(int id, AddLibroDto addLibroDto, ILogger log)
        {
            try
            {
                var libroDB = await context.Libros.Include(libro => libro.AutorLibro).FirstOrDefaultAsync(libro => libro.ID == id);
                if (libroDB == null)
                {
                    return new NotFoundObjectResult($"Book with ID: {id} not exist") { StatusCode = 404 };

                }
                libroDB = mapper.Map(addLibroDto, libroDB);
                AssignOrderAuthors(libroDB);
                await context.SaveChangesAsync();
                return new ObjectResult("") { StatusCode = 204 };

            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> UpdateLibro(int id, JsonPatchDocument<UpdateLibroDto> patchDocument, ModelStateDictionary modelState, ILogger log)
        {
            try
            {
                if (patchDocument == null)
                {
                    return new BadRequestObjectResult("You can't update a book without patchDocument") { StatusCode = 400 };
                }
                var libroDB = await context.Libros.FirstOrDefaultAsync(libro => libro.ID == id);
                if (libroDB == null)
                {
                    return new NotFoundObjectResult($"Book with ID: {id} not exist") { StatusCode = 404 };

                }
                var libroDto = mapper.Map<UpdateLibroDto>(libroDB);
                patchDocument.ApplyTo(libroDto, modelState);
                // TODO: resolver este error.
                /*var isValid = TryValidateModel(libroDto);
                if (!isValid)
                {
                    return new BadRequestObjectResult("You can't update a book, error in ModelState") { StatusCode = 400, Value = modelState };
                }*/
                mapper.Map(libroDto, libroDB);
                await context.SaveChangesAsync();
                return new ObjectResult("") { StatusCode = 204 };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        private static void AssignOrderAuthors(Libro libro)
        {
            if (libro.AutorLibro != null)
            {
                for (int i = 0; i < libro.AutorLibro.Count; i++)
                {
                    libro.AutorLibro[i].Orden = i;
                }
            }
        }
    }
}