using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIAutores.DTO;
using WebAPIAutores.Services.Shared;
using WebAPIAutores.Utils;

namespace WebAPIAutores.Filters
{
    public class HATEOASAutorFilterAttribute : HATEOASFilterAttribute
    {
        private readonly GenerateUrlsService generateUrls;

        public HATEOASAutorFilterAttribute(GenerateUrlsService generateUrls)
        {
            this.generateUrls = generateUrls;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var isInclude = isIncludeHATEOAS(context);
            if (!isInclude)
            {
                await next();
                return;
            }
            var result = context.Result as ObjectResult;
            var autorDto = result.Value as AutorDto;
            if (autorDto == null)
            {
                var autoresDto = result.Value as List<AutorDto>
                    ?? throw new ArgumentException("Se esperaba una instancia de AutorDto o List<AutorDto>");
                autoresDto.ForEach(autor => generateUrls.GenerarEnlaces(autor));
                result.Value = autoresDto;
            }
            else await generateUrls.GenerarEnlaces(autorDto);
            await next();
        }
    }
}