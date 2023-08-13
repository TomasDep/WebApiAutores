using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Core.DTO;
using WebAPIAutores.Core.Entities;
using WebAPIAutores.Core.Utils;

namespace WebAPIAutores.Core.Services
{
    public class AutorServicesImpl : IAutorServices
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutorServicesImpl(
            ApplicationDbContext context,
            IMapper mapper,
            IAuthorizationService authorizationService
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
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

        public async Task<ActionResult<AutorLibroDto>> GetAuthorById(
            int id,
            ILogger log,
            IUrlHelper urlHelper,
            ClaimsPrincipal User
        )
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
                var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");
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

        public async Task<IActionResult> GetCollectionAuthors(
            IUrlHelper urlHelper,
            ClaimsPrincipal User,
            ILogger log,
            PaginationDto paginationDto,
            HttpContext httpContext,
            string version
        )
        {
            try
            {
                var queryable = context.Autores.AsQueryable();
                await httpContext.InsertParametrosPaginationHeader(queryable);
                var autoresDB = await queryable.OrderBy(autor => autor.Nombre).Paginar(paginationDto).ToListAsync();
                if (version.Equals("V2"))
                {
                    autoresDB.ForEach(autor => autor.Nombre = autor.Nombre.ToUpper());
                }
                var autores = mapper.Map<List<AutorDto>>(autoresDB);
                log.LogInformation("Finish Get");
                return new OkObjectResult(autores);
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