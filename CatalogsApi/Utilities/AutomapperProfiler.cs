using AutoMapper;
using CatalogsApi.Dtos;
using CatalogsApi.Entites;
using CatalogsApi.Entities;

namespace CatalogsApi.Utilities
{
    public class AutomapperProfiler : Profile
    {
        public AutomapperProfiler()
        {
            CreateMap<AuthorCreationDto, Author>();
            CreateMap<Author, AuthorDto>();

            CreateMap<Author, AuthorDtoWithBooks>()
                .ForMember(a => a.Books, opt => opt.MapFrom(MapAuthorDtoBooks));

            CreateMap<BookCreationDto, Book>()
                .ForMember(b => b.AuthorsBooks, opt => opt.MapFrom(MapAuthorsBooks));
            CreateMap<Book, BookDto>();

            CreateMap<Book, BookDtoWithAuthors>()
                .ForMember(b => b.Authors, opt => opt.MapFrom(MapBookDtoAuthors));

            CreateMap<CommentCreationDto, Comment>();
            CreateMap<Comment, CommentDto>();
        }

        private List<AuthorBook> MapAuthorsBooks(BookCreationDto bookCreationDto, Book book)
        {
            var authorsBooks = new List<AuthorBook>();

            var order = 0;
            if (bookCreationDto.AuthorsIds != null)
                foreach (var authorId in bookCreationDto.AuthorsIds)
                    authorsBooks.Add(new AuthorBook() { AuthorId = authorId, Order = order++ });

            return authorsBooks;
        }

        public List<AuthorDto> MapBookDtoAuthors(Book book, BookDto bookDto)
        {
            var authorsDto = new List<AuthorDto>();

            if (book.AuthorsBooks is null) return authorsDto;

            foreach (var author in book.AuthorsBooks.OrderBy(a => a.Order))
                authorsDto.Add(new AuthorDto() { Id = author.AuthorId, Name = author.Author.Name });


            return authorsDto;
        }

        public List<BookDto> MapAuthorDtoBooks(Author author, AuthorDto authorDto)
        {
            var booksDto = new List<BookDto>();

            if (author.AuthorsBooks is null) return booksDto;

            foreach (var book in author.AuthorsBooks.OrderBy(a => a.Order))
                booksDto.Add(new BookDto() { Id = book.AuthorId, Title = book.Book.Title });


            return booksDto;
        }

    }
}
