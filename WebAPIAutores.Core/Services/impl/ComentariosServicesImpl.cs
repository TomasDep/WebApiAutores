using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Core.DTO;
using WebAPIAutores.Core.Entities;

namespace WebAPIAutores.Core.Services
{
    public class ComentariosServicesImpl : IComentariosServices
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosServicesImpl(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<ActionResult> CreateComentario(int libroId, AddComentarioDto addComentarioDto, Claim emailClaim)
        {
            try
            {
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);
                var usuarioId = usuario.Id;
                var isLibro = await context.Libros.AnyAsync(libroDB => libroDB.ID == libroId);
                if (!isLibro)
                {
                    return new NotFoundObjectResult($"Book with ID: {libroId} not exist");
                }
                var newComentario = mapper.Map<Comentario>(addComentarioDto);
                newComentario.LibroId = libroId;
                newComentario.UsuarioID = usuarioId;
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