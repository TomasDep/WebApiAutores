using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Services
{
    public class AutorServicesImpl : IAutorServices
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutorServicesImpl(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult> CreateAuthor(AddAutorDto AutorDto, ILogger log)
        {
            try
            {
                var existAuthor = await context.Autores.AnyAsync(a => a.Nombre == AutorDto.Nombre);
                if (existAuthor)
                {
                    log.LogError($"Error in Post Controller: author with name: {AutorDto.Nombre} already exists");
                    return new BadRequestObjectResult("Autor already exists") { StatusCode = 400 };
                }
                var autor = mapper.Map<Autor>(AutorDto);
                context.Add(autor);
                await context.SaveChangesAsync();
                var autorDto = mapper.Map<AutorDto>(autor);
                log.LogInformation("Finish Post");
                return new ObjectResult(autorDto) { StatusCode = 201, Value = autorDto };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult<AutorLibroDto>> GetAuthorById(int id, ILogger log)
        {
            try
            {
                var autorDB = await context.Autores
                    .Include(autor => autor.AutorLibro)
                    .ThenInclude(autorLibro => autorLibro.Libro)
                    .FirstOrDefaultAsync(autor => autor.ID == id);
                if (autorDB == null)
                {
                    log.LogError($"Error in GetById Controller: id {id} not found");
                    return new NotFoundObjectResult($"Author with ID: {id} not exist") { StatusCode = 404 };
                }
                var autor = mapper.Map<AutorLibroDto>(autorDB);
                log.LogInformation("Finish GetById");
                return new ObjectResult(autor) { StatusCode = 200, Value = autor };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult<List<AutorDto>>> GetAuthorByName(string nombre, ILogger log)
        {
            try
            {
                var autoresDB = await context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();
                if (autoresDB == null)
                {
                    log.LogError($"Error in GetByName Controller: name: {nombre} not found");
                    return new NotFoundObjectResult($"Author with Name: {nombre} not exist") { StatusCode = 404 };
                }
                var autores = mapper.Map<List<AutorDto>>(autoresDB);
                log.LogInformation("Finish GetByName");
                return new ObjectResult(autores) { StatusCode = 200, Value = autores };

            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<List<AutorDto>> GetCollectionAuthors(ILogger log)
        {
            try
            {
                var autoresDB = await context.Autores.ToListAsync();
                var autores = mapper.Map<List<AutorDto>>(autoresDB);
                log.LogInformation("Finish Get");
                return autores;
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> RemoveAuthor(int id, ILogger log)
        {
            try
            {
                var exist = await context.Autores.AnyAsync(autor => autor.ID == id);
                if (!exist)
                {
                    log.LogError($"Error in Delete Controller: autor with id: {id} not found");
                    return new NotFoundObjectResult($"Author with ID: {id} not exist") { StatusCode = 404 };
                }
                context.Remove(new Autor() { ID = id });
                await context.SaveChangesAsync();
                log.LogInformation("Finish Delete");
                return new ObjectResult("") { StatusCode = 204 };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> UpdateAuthor(AddAutorDto addAutorDto, int id, ILogger log)
        {
            try
            {
                var existAuthor = await context.Autores.AnyAsync(a => a.ID == id);
                if (!existAuthor)
                {
                    log.LogError($"Error in Put Controller: id {id} not found");
                    return new NotFoundObjectResult($"Author with ID: {id} not exist") { StatusCode = 404 };
                }
                var autor = mapper.Map<Autor>(addAutorDto);
                autor.ID = id;
                context.Update(autor);
                await context.SaveChangesAsync();
                log.LogInformation("Finish Put");
                return new ObjectResult("") { StatusCode = 204 };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }
    }
}