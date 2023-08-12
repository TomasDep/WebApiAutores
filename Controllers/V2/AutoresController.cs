using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;
using WebAPIAutores.Filters;
using WebAPIAutores.Services;
using WebAPIAutores.Utils;

namespace WebAPIAutores.Controllers.V2
{
    [ApiController]
    [Route("api/autores")]
    //[Route("api/v2/autores")]
    [HeadersPresent("x-version", "2")]
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
        [HttpGet(Name = "obtenerColleccionAutoresV2")]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public Task<IActionResult> Get([FromQuery] PaginationDto paginationDto)
        {
            log.LogInformation("Init Get");
            return autorServices.GetCollectionAuthors(Url, User, log, paginationDto, HttpContext, "V2");
        }
    }
}