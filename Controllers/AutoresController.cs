using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ILogger<AutoresController> log;
        private readonly IAutorServices autorServices;

        public AutoresController(ILogger<AutoresController> log, IAutorServices autorServices)
        {
            this.log = log;
            this.autorServices = autorServices;
        }

        [AllowAnonymous]
        [HttpGet(Name = "obtenerColleccionAutores")]
        public Task<IActionResult> Get([FromQuery] bool includeHATEOAS = true)
        {
            log.LogInformation("Init Get");
            return autorServices.GetCollectionAuthors(Url, User, includeHATEOAS, log);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "obtenerAutorPorId")]
        public Task<ActionResult<AutorLibroDto>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            return autorServices.GetAuthorById(id, log, Url, User);
        }

        [AllowAnonymous]
        [HttpGet("{nombre}", Name = "obtenerAutorPorNombre")]
        public Task<ActionResult<List<AutorDto>>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByName");
            return autorServices.GetAuthorByName(nombre, log);
        }

        [HttpPost(Name = "crearAutor")]
        public Task<ActionResult> Post([FromBody] AddAutorDto AutorDto)
        {
            log.LogInformation("Init Post");
            return autorServices.CreateAuthor(AutorDto, log);
        }

        [HttpPut("{id:int}", Name = "actualizarAutor")]
        public Task<ActionResult> Put(AddAutorDto addAutorDto, int id)
        {
            log.LogInformation("Init Put");
            return autorServices.UpdateAuthor(addAutorDto, id, log);
        }

        [HttpDelete("{id:int}", Name = "borrarAutor")]
        public Task<ActionResult> Delete(int id)
        {
            log.LogInformation("Init Delete");
            return autorServices.RemoveAuthor(id, log);
        }
    }
}