using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public readonly ILogger<AutoresController> log;


        public LibroController(ApplicationDbContext context, ILogger<AutoresController> log)
        {
            this.context = context;
            this.log = log;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            log.LogInformation("Init Get");
            var booksCollection = await context.Libros.Include(lib => lib.Autor).FirstOrDefaultAsync(libro => libro.ID == id);
            log.LogInformation("Finish Get");
            return booksCollection;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Libro libro)
        {
            log.LogInformation("Init Post");
            var existAutor = await context.Autores.AnyAsync(autor => autor.ID == libro.AutorID);

            if (!existAutor)
            {
                log.LogError($"Error in Post Controller: name: {libro.AutorID} not found");
                return BadRequest($"Autor not exist with ID: {libro.AutorID}");
            }

            context.Add(libro);
            await context.SaveChangesAsync();

            log.LogInformation("Finish Post");
            return Ok();
        }
    }
}