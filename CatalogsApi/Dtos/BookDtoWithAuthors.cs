namespace CatalogsApi.Dtos
{
    public class BookDtoWithAuthors : BookDto
    {
        public ICollection<AuthorDto> Authors { get; set; }
    }
}
