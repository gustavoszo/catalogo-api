using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.Validations
{
    public class MaiuscFirstLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("A primeira letra do produto deve ser maiuscula");
            }
            return ValidationResult.Success;
        }

    }
}
