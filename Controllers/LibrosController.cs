using AutoMapper;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDto>> Get(int id)
        {
            log.LogInformation("Init Get");
            var librosDB = await context.Libros.FirstOrDefaultAsync(libro => libro.ID == id);
            var libros = mapper.Map<LibroDto>(librosDB);
            log.LogInformation("Finish Get");
            return Ok(libros);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddLibroDto libroDto)
        {
            log.LogInformation("Init Post");
            if (libroDto.AutoresIds == null)
            {
                return BadRequest("You can't create a book without authors");
            }
            var autoresIds = await context.Autores.Where(autorDB => libroDto.AutoresIds.Contains(autorDB.ID)).Select(autor => autor.ID).ToListAsync();
            if (libroDto.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("Not existing authors");
            }
            var newLibro = mapper.Map<Libro>(libroDto);
            if (newLibro.AutorLibro != null)
            {
                for (int i = 0; i < newLibro.AutorLibro.Count; i++)
                {
                    newLibro.AutorLibro[i].Orden = i;
                }
            }
            context.Add(newLibro);
            await context.SaveChangesAsync();
            log.LogInformation("Finish Post");
            return Ok();
        }
    }
}