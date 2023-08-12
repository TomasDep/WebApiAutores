using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Filters;
using WebAPIAutores.Services;
using WebAPIAutores.Utils;

namespace WebAPIAutores.Controllers.V1
{
    [ApiController]
    [Route("api/autores")]
    //[Route("api/v1/autores")]
    [HeadersPresent("x-version", "1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
        [HttpGet(Name = "obtenerColleccionAutoresV1")]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public Task<IActionResult> Get([FromQuery] PaginationDto paginationDto)
        {
            log.LogInformation("Init Get");
            return autorServices.GetCollectionAuthors(Url, User, log, paginationDto, HttpContext, "V1");
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "obtenerAutorPorIdV1")]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public Task<ActionResult<AutorLibroDto>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            return autorServices.GetAuthorById(id, log, Url, User);
        }

        [AllowAnonymous]
        [HttpGet("{nombre}", Name = "obtenerAutorPorNombreV1")]
        public Task<ActionResult<List<AutorDto>>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByNameV1");
            return autorServices.GetAuthorByName(nombre, log);
        }

        [HttpPost(Name = "crearAutorV1")]
        public Task<ActionResult> Post([FromBody] AddAutorDto AutorDto)
        {
            log.LogInformation("Init Post");
            return autorServices.CreateAuthor(AutorDto, log);
        }

        [HttpPut("{id:int}", Name = "actualizarAutorV1")]
        public Task<ActionResult> Put(AddAutorDto addAutorDto, int id)
        {
            log.LogInformation("Init PutV1");
            return autorServices.UpdateAuthor(addAutorDto, id, log);
        }

        [HttpDelete("{id:int}", Name = "borrarAutorV1")]
        public Task<ActionResult> Delete(int id)
        {
            log.LogInformation("Init Delete");
            return autorServices.RemoveAuthor(id, log);
        }
    }
}