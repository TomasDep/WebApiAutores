namespace WebAPIAutores.DTO
{
    public class DataHATEOASDto
    {
        public string Enlace { get; private set; }
        public string Descripcion { get; private set; }
        public string Metodo { get; private set; }

        public DataHATEOASDto(string enlace, string descripcion, string metodo)
        {
            Enlace = enlace;
            Descripcion = descripcion;
            Metodo = metodo;
        }
    }
}