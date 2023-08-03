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

        public LibroController(ApplicationDbContext context) {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id) {
            return await context.Libros.Include(lib => lib.Autor).FirstOrDefaultAsync(libro => libro.ID == id);            
        }

        [HttpPost]
        public async Task<IActionResult> Post(Libro libro) {
            var existAutor = await context.Autores.AnyAsync(autor => autor.ID == libro.AutorID);

            if (!existAutor) return BadRequest($"Autor not exist with ID: {libro.AutorID}");

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}