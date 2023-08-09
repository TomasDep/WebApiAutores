using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ILogger<AutoresController> log;
        private readonly IAutorServices autorServices;

        public AutoresController(ILogger<AutoresController> log, IAutorServices autorServices)
        {
            this.log = log;
            this.autorServices = autorServices;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<List<AutorDto>> Get()
        {
            log.LogInformation("Init Get");
            return autorServices.GetCollectionAuthors(log);
        }

        [HttpGet("{id:int}", Name = "GetAutorById")]
        [AllowAnonymous]
        public Task<ActionResult<AutorLibroDto>> GetById(int id)
        {
            log.LogInformation("Init GetById");
            return autorServices.GetAuthorById(id, log);
        }

        [HttpGet("{nombre}")]
        public Task<ActionResult<List<AutorDto>>> GetByName([FromRoute] string nombre)
        {
            log.LogInformation("Init GetByName");
            return autorServices.GetAuthorByName(nombre, log);
        }

        [HttpPost]
        public Task<ActionResult> Post([FromBody] AddAutorDto AutorDto)
        {
            log.LogInformation("Init Post");
            return autorServices.CreateAuthor(AutorDto, log);
        }

        [HttpPut("{id:int}")]
        public Task<ActionResult> Put(AddAutorDto addAutorDto, int id)
        {
            log.LogInformation("Init Put");
            return autorServices.UpdateAuthor(addAutorDto, id, log);
        }

        [HttpDelete("{id:int}")]
        public Task<ActionResult> Delete(int id)
        {
            log.LogInformation("Init Delete");
            return autorServices.RemoveAuthor(id, log);
        }
    }
}