using AutoMapper;
using CatalogsApi.Context;
using CatalogsApi.Dtos;
using CatalogsApi.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogsApi.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //[HttpGet("guid")]
        //[ResponseCache(Duration = 60)]
        //[ServiceFilter(typeof(ActionFilter))]
        //public ActionResult GetGuid()
        //{
        //    return Ok(Guid.NewGuid());
        //}

        [HttpGet("{id:int}", Name = "GetByIdAuthor")]
        public async Task<ActionResult<AuthorDtoWithBooks>> GetById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.AuthorsBooks)
                .ThenInclude(ab => ab.Book)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null) return NotFound();

            return Ok(_mapper.Map<AuthorDtoWithBooks>(author));
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<AuthorDto>>> GetAll()
        {
            var authors = await _context.Authors.ToListAsync();

            return Ok(_mapper.Map<List<AuthorDto>>(authors));
        }

        [HttpPost]
        public async Task<ActionResult> Post(AuthorCreationDto authorCreationDto)
        {
            var author = _mapper.Map<Author>(authorCreationDto);

            _context.Add(author);
            await _context.SaveChangesAsync();


            //return Ok();

            var authorDto = _mapper.Map<AuthorDto>(author);
            return CreatedAtRoute("GetByIdAuthor", new { id = author.Id }, authorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AuthorCreationDto authorCreationDto)
        {
            var authorExists = await _context.Authors.AnyAsync(x => x.Id == id);
            if (!authorExists) return NotFound();

            var author = _mapper.Map<Author>(authorCreationDto);
            author.Id = id;



            _context.Update(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var authorDb = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);

            if (authorDb == null) return NotFound();

            //_context.Remove(new Author() { Id = id });
            _context.Remove(authorDb);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
