namespace WebAPIAutores.DTO
{
    public class PaginationDto
    {
        public int Pagina { get; init; }
        public int recordPorPagina = 10;
        public readonly int cantidadMaximaPorPagina = 50;

        public int RecordPorPagina
        {
            get
            {
                return recordPorPagina;
            }
            set
            {
                recordPorPagina = (value > cantidadMaximaPorPagina) ? cantidadMaximaPorPagina : value;
            }
        }
    }
}