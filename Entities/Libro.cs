using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validators;

namespace WebAPIAutores.Entities
{
    public class Libro
    {
        public int ID { get; set; }
        [Required]
        [FirstLetterUppercase]
        public string Titulo { get; set; }
        public int AutorID { get; set; }
        public Autor Autor{ get; set; }
    }
}