using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.Core.DTO;

namespace WebAPIAutores.Core.Services
{
    public interface IComentariosServices
    {
        Task<ActionResult<List<ComentarioDto>>> GetCollectionComentarios(int libroId);
        Task<ActionResult<ComentarioDto>> GetComentarioById(int id);
        Task<ActionResult> CreateComentario(int libroId, AddComentarioDto addComentarioDto, Claim emailClaim);
        Task<ActionResult> UpdateComentario(int libroId, int id, AddComentarioDto addComentarioDto);
    }
}