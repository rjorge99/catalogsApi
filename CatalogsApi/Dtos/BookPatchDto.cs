using CatalogsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Dtos
{
    public class BookPatchDto
    {
        [Required]
        [IsFirstCharacterUpperCase]
        [StringLength(250)]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
