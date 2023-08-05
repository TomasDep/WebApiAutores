using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTO
{
    public class AutorDTO
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }


    public class AddAutorDTO
    {
        [Required(ErrorMessage = "The name of author is required")]
        [StringLength(maximumLength: 250)]
        public string Nombre { get; set; }
        [Range(18, 120)]
        public int Edad { get; set; }
    }
}