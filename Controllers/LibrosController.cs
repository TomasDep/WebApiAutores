using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : ControllerBase
    {
        private readonly ILogger<AutoresController> log;
        private readonly ILibrosServices librosServices;

        public LibroController(ILibrosServices librosServices, ILogger<AutoresController> log)
        {
            this.librosServices = librosServices;
            this.log = log;
        }

        [HttpGet("{id:int}", Name = "obtenerLibroPorId")]
        public Task<ActionResult<LibroAutorDto>> GetById(int id)
        {
            log.LogInformation("Init Get");
            return librosServices.GetLibroById(id, log);
        }

        [HttpPost(Name = "crearLibro")]
        public Task<IActionResult> Post(AddLibroDto addlibroDto)
        {
            log.LogInformation("Init Post");
            return librosServices.CreateLibro(addlibroDto, log);
        }

        [HttpPut("{id:int}", Name = "actualizarLibro")]
        public async Task<ActionResult> Put(int id, AddLibroDto addLibroDto)
        {
            return await librosServices.UpdateLibro(id, addLibroDto, log);
        }

        [HttpPatch("{id:int}", Name = "actualizarDatosLibro")]
        public Task<ActionResult> Patch(int id, JsonPatchDocument<UpdateLibroDto> patchDocument)
        {
            return librosServices.UpdateLibro(id, patchDocument, ModelState, log);
        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public Task<ActionResult> DeleteAsync(int id)
        {
            return librosServices.RemoveLibro(id, log);
        }
    }
}