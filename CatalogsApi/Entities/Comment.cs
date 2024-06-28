using CatalogsApi.Entites;

namespace CatalogsApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
