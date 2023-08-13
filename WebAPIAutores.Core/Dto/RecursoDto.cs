namespace WebAPIAutores.Core.DTO
{
    public class RecursoDto
    {
        public List<DataHATEOASDto> Enlaces { get; set; } = new List<DataHATEOASDto>();
    }

    public class ColleccionRecursos<T> : RecursoDto where T : RecursoDto
    {
        public List<T> Valores { get; set; }
    }
}