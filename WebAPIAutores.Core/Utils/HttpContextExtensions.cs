using Microsoft.EntityFrameworkCore;

namespace WebAPIAutores.Core.Utils
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametrosPaginationHeader<T>(
            this HttpContext httpContext,
            IQueryable<T> queryable
        )
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
        }
    }
}