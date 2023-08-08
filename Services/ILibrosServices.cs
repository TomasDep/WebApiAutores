using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services
{
    public interface ILibrosServices
    {
        Task<ActionResult<LibroAutorDto>> GetLibroById(int id, ILogger log);
        Task<IActionResult> CreateLibro(AddLibroDto addlibroDto, ILogger log);
        Task<ActionResult> UpdateLibro(int id, AddLibroDto addLibroDto, ILogger log);
        Task<ActionResult> UpdateLibro(int id, JsonPatchDocument<UpdateLibroDto> patchDocument, ModelStateDictionary modelState, ILogger log);
        Task<ActionResult> RemoveLibro(int id, ILogger log);
    }
}