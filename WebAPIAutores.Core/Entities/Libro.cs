using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Core.Validators;

namespace WebAPIAutores.Core.Entities
{
    public class Libro
    {
        public int ID { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        [FirstLetterUppercase]
        public string Titulo { get; set; }
        public int AutorID { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<AutorLibro> AutorLibro { get; set; }
        public List<Comentario> Comentarios { get; set; }
    }
}