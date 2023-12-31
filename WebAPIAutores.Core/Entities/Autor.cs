using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Core.Entities
{
  public class Autor : IValidatableObject
  {
    public int ID { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public List<AutorLibro> AutorLibro { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (!string.IsNullOrEmpty(Nombre))
      {
        var firstLetter = Nombre[0].ToString();
        if (firstLetter != firstLetter.ToUpper())
          yield return new ValidationResult("The first letter most be Uppercase", new String[] { nameof(Nombre) });
      }
    }
  }
}