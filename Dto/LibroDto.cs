using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validators;

namespace WebAPIAutores.DTO
{
    public class LibroDto
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }

    public class AddLibroDto
    {
        [Required]
        [StringLength(maximumLength: 250)]
        [FirstLetterUppercase]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }

    public class UpdateLibroDto
    {
        [Required]
        [StringLength(maximumLength: 250)]
        [FirstLetterUppercase]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}