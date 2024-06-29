using AutoMapper;
using CatalogsApi.Context;
using CatalogsApi.Dtos;
using CatalogsApi.Entites;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "GetByIdBook")]
        public async Task<ActionResult<BookDtoWithAuthors>> GetById(int id)
        {
            var book = await _context.Books
                //.Include(b => b.Comments)
                .Include(b => b.AuthorsBooks)
                .ThenInclude(ab => ab.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null) return NotFound();

            return Ok(_mapper.Map<BookDtoWithAuthors>(book));
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

            var book = _mapper.Map<Book>(bookCreationDto);
            _context.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = _mapper.Map<BookDto>(book);
            return CreatedAtRoute("GetByIdBook", new { id = book.Id }, bookDto);
            //return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, BookCreationDto bookCreationDto)
        {
            var bookDb = await _context.Books
                .Include(b => b.AuthorsBooks)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookDb is null) return NotFound();

            _mapper.Map(bookCreationDto, bookDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<BookPatchDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest();

            var bookDb = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (bookDb is null) return NotFound();

            var bookPatchDto = _mapper.Map<BookPatchDto>(bookDb);
            patchDocument.ApplyTo(bookPatchDto, ModelState);

            if (!TryValidateModel(bookPatchDto)) return BadRequest(ModelState);

            _mapper.Map(bookPatchDto, bookDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookExists = await _context.Books.AnyAsync(x => x.Id == id);

            if (!bookExists) return NotFound();

            _context.Remove(new Book() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
