using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> log;
        private readonly IMapper mapper;

        public LibroController(
            ApplicationDbContext context,
            ILogger<AutoresController> log,
            IMapper mapper
        )
        {
            this.context = context;
            this.log = log;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetLibroById")]
        public async Task<ActionResult<LibroAutorDto>> GetById(int id)
        {
            log.LogInformation("Init Get");
            var librosDB = await context.Libros.Include(lib => lib.AutorLibro)
                .ThenInclude(autorLibroBD => autorLibroBD.Autor)
                .FirstOrDefaultAsync(libro => libro.ID == id);
            if (librosDB == null)
            {
                return NotFound();
            }
            librosDB.AutorLibro = librosDB.AutorLibro.OrderBy(autlib => autlib.Orden).ToList();
            var libros = mapper.Map<LibroAutorDto>(librosDB);
            log.LogInformation("Finish Get");
            return Ok(libros);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddLibroDto addlibroDto)
        {
            log.LogInformation("Init Post");
            if (addlibroDto.AutoresIds == null)
            {
                return BadRequest("You can't create a book without authors");
            }
            var autoresIds = await context.Autores.Where(autorDB => addlibroDto.AutoresIds.Contains(autorDB.ID)).Select(autor => autor.ID).ToListAsync();
            if (addlibroDto.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("Not existing authors");
            }
            var newLibro = mapper.Map<Libro>(addlibroDto);
            AssignOrderAuthors(newLibro);
            context.Add(newLibro);
            await context.SaveChangesAsync();
            var libroDto = mapper.Map<LibroDto>(newLibro);
            log.LogInformation("Finish Post");
            return CreatedAtRoute("GetLibroById", new { id = libroDto.ID }, libroDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AddLibroDto addLibroDto)
        {
            var libroDB = await context.Libros.Include(libro => libro.AutorLibro).FirstOrDefaultAsync(libro => libro.ID == id);
            if (libroDB == null)
            {
                return NotFound();
            }
            libroDB = mapper.Map(addLibroDto, libroDB);
            AssignOrderAuthors(libroDB);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<UpdateLibroDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var libroDB = await context.Libros.FirstOrDefaultAsync(libro => libro.ID == id);
            if (libroDB == null)
            {
                return NotFound();
            }
            var libroDto = mapper.Map<UpdateLibroDto>(libroDB);
            patchDocument.ApplyTo(libroDto, ModelState);
            var isValid = TryValidateModel(libroDto);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(libroDto, libroDB);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existLibro = await context.Libros.AnyAsync(libro => libro.ID == id);
            if (!existLibro)
            {
                return NotFound();
            }
            context.Remove(new Libro() { ID = id });
            await context.SaveChangesAsync();
            return Ok();
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