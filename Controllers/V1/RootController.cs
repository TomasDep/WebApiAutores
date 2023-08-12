using System.Collections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "getRootV1")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DataHATEOASDto>>> Get()
        {
            var dataHateoas = new List<DataHATEOASDto>();
            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");
            dataHateoas.Add(new DataHATEOASDto(enlace: Url.Link("getRoot", new { }), descripcion: "Self", metodo: "GET"));
            dataHateoas.Add(new DataHATEOASDto(enlace: Url.Link("obtenerColleccionAutores", new { }), descripcion: "autores", metodo: "GET"));
            if (isAdmin.Succeeded)
            {
                dataHateoas.Add(new DataHATEOASDto(enlace: Url.Link("crearAutor", new { }), descripcion: "autores-crear", metodo: "POST"));
                dataHateoas.Add(new DataHATEOASDto(enlace: Url.Link("crearLibro", new { }), descripcion: "libro-crear", metodo: "POST"));
            }
            return dataHateoas;
        }
    }
}