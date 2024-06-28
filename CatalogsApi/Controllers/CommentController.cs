﻿using AutoMapper;
using CatalogsApi.Context;
using CatalogsApi.Dtos;
using CatalogsApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogsApi.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDto>>> Get(int bookId)
        {
            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!bookExists) return NotFound("Book not found");

            var comments = await _context.Comments.Where(c => c.BookId == bookId).ToListAsync();
            return Ok(_mapper.Map<List<CommentDto>>(comments));
        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, CommentCreationDto commentCreationDto)
        {
            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!bookExists) return NotFound("Book not found");

            var comment = _mapper.Map<Comment>(commentCreationDto);
            comment.BookId = bookId;

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
