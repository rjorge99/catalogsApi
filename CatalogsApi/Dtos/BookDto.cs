﻿namespace CatalogsApi.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        //public ICollection<CommentDto> Comments { get; set; }
    }
}
