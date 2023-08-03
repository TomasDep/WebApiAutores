namespace WebAPIAutores.Entities
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public int AutorID { get; set; }
        public Autor Autor{ get; set; }
    }
}