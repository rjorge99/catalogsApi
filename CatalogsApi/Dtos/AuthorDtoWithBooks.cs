﻿namespace CatalogsApi.Dtos
{
    public class AuthorDtoWithBooks : AuthorDto
    {
        public List<BookDto> Books { get; set; }
    }
}
