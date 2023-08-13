using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Core.Validators
{
    public class FirstLetterUppercase : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var firstWord = value.ToString()[0].ToString();

            if (firstWord != firstWord.ToUpper())
                return new ValidationResult("The first char needs to be uppercase");

            return ValidationResult.Success;
        }
    }
}