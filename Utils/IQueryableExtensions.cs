using WebAPIAutores.DTO;

namespace WebAPIAutores.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable
                .Skip((paginationDto.Pagina - 1) * paginationDto.RecordPorPagina)
                .Take(paginationDto.RecordPorPagina);
        }
    }
}