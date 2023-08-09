using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTO;
using WebAPIAutores.Entities;

namespace WebAPIAutores.Services
{
    public class ComentariosServicesImpl : IComentariosServices
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosServicesImpl(ApplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult> CreateComentario(int libroId, AddComentarioDto addComentarioDto)
        {
            try
            {
                var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
                if (!isLibro)
                {
                    return new NotFoundObjectResult($"Book with ID: {libroId} not exist");
                }
                var newComentario = mapper.Map<Comentario>(addComentarioDto);
                newComentario.LibroId = libroId;
                context.Add(newComentario);
                await context.SaveChangesAsync();
                var comentarioDto = mapper.Map<ComentarioDto>(newComentario);
                return new OkObjectResult(comentarioDto);                
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult<List<ComentarioDto>>> GetCollectionComentarios(int libroId)
        {
            try
            {    
                var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
                if (!isLibro)
                {
                    return new NotFoundObjectResult($"Book with ID: {libroId} not exist");
                }
                var listComentariosDB = await context.Comentarios.Where(comentario => comentario.LibroId == libroId).ToListAsync();
                var listComentarios = mapper.Map<List<ComentarioDto>>(listComentariosDB);
                return listComentarios;
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult<ComentarioDto>> GetComentarioById(int id)
        {
            try
            {
                var comentarioBD = await context.Comentarios.FirstOrDefaultAsync(comentario => comentario.ID == id);
                if (comentarioBD == null)
                {
                    return new NotFoundObjectResult($"Comment with ID: {id} not exist");
                }
                var comentarioDto = mapper.Map<ComentarioDto>(comentarioBD);
                return comentarioDto;    
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }

        public async Task<ActionResult> UpdateComentario(int libroId, int id, AddComentarioDto addComentarioDto)
        {
            try
            {    
                var isLibro = await this.context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
                if (!isLibro)
                {
                    return new NotFoundObjectResult($"Book with ID: {libroId} not exist");
                }
                var existComentario = await context.Comentarios.AnyAsync(comentadioDB => comentadioDB.ID == id);
                if (!existComentario)
                {
                    return new NotFoundObjectResult($"Comment with ID: {id} not exist");
                }
                var comentario = mapper.Map<Comentario>(addComentarioDto);
                comentario.ID = id;
                comentario.LibroId = libroId;
                context.Update(comentario);
                await context.SaveChangesAsync();
                return new ObjectResult("") { StatusCode = 204 };
            }
            catch (Exception e)
            {
                throw new Exception($"Core Exception : {e.Message}");
            }
        }
    }
}