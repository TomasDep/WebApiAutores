using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<AutoresController> log;
        private readonly IMapper mapper;

        public ComentariosController(
            ApplicationDbContext context,
            ILogger<AutoresController> log,
            IMapper mapper
        )
        {
            this.context = context;
            this.log = log;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDto>>> Get(int libroId)
        {
            var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
            if (!isLibro)
            {
                return NotFound();
            }
            var listComentariosDB = await context.Comentarios.Where(comentario => comentario.LibroId == libroId).ToListAsync();
            var listComentarios = mapper.Map<List<ComentarioDto>>(listComentariosDB);
            return listComentarios;
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, AddComentarioDto comentarioDto)
        {
            var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
            if (!isLibro)
            {
                return NotFound();
            }
            var newComentario = mapper.Map<Comentario>(comentarioDto);
            newComentario.LibroId = libroId;
            context.Add(newComentario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}