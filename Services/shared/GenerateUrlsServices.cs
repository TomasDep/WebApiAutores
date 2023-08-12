using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebAPIAutores.DTO;

namespace WebAPIAutores.Services.Shared
{
    public class GenerateUrlsService
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContext;
        private readonly IActionContextAccessor actionContext;

        public GenerateUrlsService(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContext,
            IActionContextAccessor actionContext
        )
        {
            this.authorizationService = authorizationService;
            this.httpContext = httpContext;
            this.actionContext = actionContext;
        }

        private IUrlHelper URLHelperFactory()
        {
            var factory = httpContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext.ActionContext);
        }

        private async Task<bool> IsAdmin()
        {
            var http = httpContext.HttpContext;
            var result = await authorizationService.AuthorizeAsync(http.User, "isAdmin");
            return result.Succeeded;
        }

        public async Task GenerarEnlaces(AutorDto autorDto)
        {
            var isAdministrator = await IsAdmin();
            var url = URLHelperFactory();

            autorDto.Enlaces.Add(new DataHATEOASDto(
                enlace: url.Link("obtenerAutorPorId", new { id = autorDto.ID }),
                descripcion: "self",
                metodo: "GET"
            ));
            if (isAdministrator)
            {
                autorDto.Enlaces.Add(new DataHATEOASDto(
                    enlace: url.Link("actualizarAutor", new { id = autorDto.ID }),
                    descripcion: "autor-actualizar",
                    metodo: "PUT"
                ));
                autorDto.Enlaces.Add(new DataHATEOASDto(
                    enlace: url.Link("borrarAutor", new { id = autorDto.ID }),
                    descripcion: "self",
                    metodo: "DELETE"
                ));
            }
        }
    }
}