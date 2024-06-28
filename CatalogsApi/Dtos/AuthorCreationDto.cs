using CatalogsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Dtos
{
    public class AuthorCreationDto
    {
        [Required]
        [IsFirstCharacterUpperCase]
        [StringLength(120, ErrorMessage = "The field {0} should not have more than {1} characters.")]
        public string Name { get; set; }
    }
}
