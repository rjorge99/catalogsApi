using CatalogsApi.Entities;
using CatalogsApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace CatalogsApi.Entites
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [IsFirstCharacterUpperCase]
        [StringLength(250)]
        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
