using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Services;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ILogger<AutoresController> log;
        private readonly IComentariosServices comentariosServices;

        public ComentariosController(IComentariosServices comentariosServices, ILogger<AutoresController> log)
        {
            this.comentariosServices = comentariosServices;
            this.log = log;
        }

        [HttpGet(Name = "obtenerColleccionComentarios")]
        public Task<ActionResult<List<ComentarioDto>>> Get(int libroId)
        {
            return comentariosServices.GetCollectionComentarios(libroId);
        }

        [HttpGet("{id:int}", Name = "obtenerComentarioPorId")]
        public Task<ActionResult<ComentarioDto>> GetById(int id)
        {
            return comentariosServices.GetComentarioById(id);
        }

        [HttpPost(Name = "crearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ActionResult> Post(int libroId, AddComentarioDto addComentarioDto)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            return comentariosServices.CreateComentario(libroId, addComentarioDto, emailClaim);
        }

        [HttpPut("{id:int}", Name = "actualizarComentario")]
        public Task<ActionResult> Put(int libroId, int id, AddComentarioDto addComentarioDto)
        {
            return comentariosServices.UpdateComentario(libroId, id, addComentarioDto);
        }
    }
}