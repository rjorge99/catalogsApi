using AutoMapper;
using CatalogsApi.Context;
using CatalogsApi.Dtos;
using CatalogsApi.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogsApi.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _context.Books
                //.Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null) return NotFound();

            return Ok(_mapper.Map<BookDto>(book));
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(_mapper.Map<List<BookDto>>(books));
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookCreationDto bookCreationDto)
        {
            if (bookCreationDto.AuthorsIds is null) return BadRequest("Author not found");

            var authorsIds = await _context.Authors
                .Where(a => bookCreationDto.AuthorsIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync();

            if (authorsIds.Count() != bookCreationDto.AuthorsIds.Count())
                return BadRequest("Author not found");

            //var authorExists = await _context.Authors.AnyAsync(a => a.Id == book.AuthorId);
            //if (!authorExists) return BadRequest("Author not found");

            _context.Add(_mapper.Map<Book>(bookCreationDto));
            await _context.SaveChangesAsync();
            return Ok();
        }
        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(int id, Author author)
        //{
        //    if (author.Id != id) return BadRequest("Invalid Id");

        //    var authorDb = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        //    if (authorDb == null) return NotFound();

        //    _context.Update(author);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}


        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var authorDb = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);

        //    if (authorDb == null) return NotFound();

        //    //_context.Remove(new Author() { Id = id });
        //    _context.Remove(authorDb);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

    }
}
