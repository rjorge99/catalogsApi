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

            CreateMap<BookCreationDto, Book>()
                .ForMember(b => b.AuthorsBooks, opt => opt.MapFrom(MapAuthorsBooks));
            CreateMap<Book, BookDto>();

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
    }
}
