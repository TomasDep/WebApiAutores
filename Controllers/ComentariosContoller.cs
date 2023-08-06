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

        [HttpGet("{id:int}", Name = "GetComentarioById")]
        public async Task<ActionResult<ComentarioDto>> GetById(int id)
        {
            var comentarioBD = await context.Comentarios.FirstOrDefaultAsync(comentario => comentario.ID == id);
            if (comentarioBD == null) return NotFound();
            var comentarioDto = mapper.Map<ComentarioDto>(comentarioBD);
            return comentarioDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(int libroId, AddComentarioDto addComentarioDto)
        {
            var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
            if (!isLibro)
            {
                return NotFound();
            }
            var newComentario = mapper.Map<Comentario>(addComentarioDto);
            newComentario.LibroId = libroId;
            context.Add(newComentario);
            await context.SaveChangesAsync();
            var comentarioDto = mapper.Map<ComentarioDto>(newComentario);
            return CreatedAtRoute("GetComentarioById", new { id = newComentario.ID, libroId = libroId }, comentarioDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, AddComentarioDto addComentarioDto)
        {
            var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
            if (!isLibro)
            {
                return NotFound();
            }
            var existComentario = await context.Comentarios.AnyAsync(comentadioDB => comentadioDB.ID == id);
            if (!existComentario)
            {
                return NotFound();
            }
            var comentario = mapper.Map<Comentario>(addComentarioDto);
            comentario.ID = id;
            comentario.LibroId = libroId;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}