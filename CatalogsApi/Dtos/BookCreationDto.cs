using CatalogsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Dtos
{
    public class BookCreationDto
    {
        [Required]
        [IsFirstCharacterUpperCase]
        [StringLength(250)]
        public string Title { get; set; }

        public ICollection<int> AuthorsIds { get; set; }
    }
}
