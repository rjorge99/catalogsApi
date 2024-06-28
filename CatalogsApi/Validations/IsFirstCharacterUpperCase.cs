using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Validations
{
    public class IsFirstCharacterUpperCase : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Success;

            var firstCharacter = value.ToString()[0].ToString();

            if (firstCharacter != firstCharacter.ToUpper())
                return new ValidationResult("First character is not upper case");

            return ValidationResult.Success;
        }
    }
}
