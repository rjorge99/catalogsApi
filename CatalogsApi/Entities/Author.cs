using CatalogsApi.Entities;
using CatalogsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Entites
{
    public class Author // : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [IsFirstCharacterUpperCase]
        [StringLength(120, ErrorMessage = "The field {0} should not have more than {1} characters.")]
        public string Name { get; set; }

        public ICollection<AuthorBook> AuthorsBooks { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrEmpty(Name))
        //    {
        //        var firstCharacter = Name?[0].ToString();

        //        if (firstCharacter != firstCharacter.ToUpper())
        //            yield return new ValidationResult("Name is required", new[] { nameof(Name) });
        //    }
        //}
    }
}
