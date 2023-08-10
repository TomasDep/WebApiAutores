using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services
{
    public interface IAutorServices
    {
        Task<IActionResult> GetCollectionAuthors(IUrlHelper urlHelper, ClaimsPrincipal User, bool includeHATEOAS, ILogger log);
        Task<ActionResult<AutorLibroDto>> GetAuthorById(int id, ILogger log, IUrlHelper urlHelper, ClaimsPrincipal User);
        Task<ActionResult<List<AutorDto>>> GetAuthorByName(string nombre, ILogger log);
        Task<ActionResult> CreateAuthor(AddAutorDto AutorDto, ILogger log);
        Task<ActionResult> UpdateAuthor(AddAutorDto addAutorDto, int id, ILogger log);
        Task<ActionResult> RemoveAuthor(int id, ILogger log);
    }
}