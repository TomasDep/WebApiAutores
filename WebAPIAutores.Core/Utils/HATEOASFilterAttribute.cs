using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Core.Utils
{
    public class HATEOASFilterAttribute : ResultFilterAttribute
    {
        protected bool isIncludeHATEOAS(ResultExecutingContext context)
        {
            var resultado = context.Result as ObjectResult;
            if (isSuccessResult(resultado)) return false;
            var header = context.HttpContext.Request.Headers["incluirHATEOAS"];
            if (header.Count == 0) return false;
            var value = header[0];
            if (!value.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) return false;
            return true;
        }

        private bool isSuccessResult(ObjectResult result)
        {
            if (result == null || result.Value == null) return false;
            if (result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2")) return false;
            return true;
        }
    }
}