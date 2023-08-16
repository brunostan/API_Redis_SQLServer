using BookStore.Data;
using BookStore.Entities;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly CacheService _cacheService;
        private readonly DatabaseContext _context;

        public BooksController(IDistributedCache cache, DatabaseContext context)
        {
            _cacheService = new CacheService(cache);
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var key = "Books:All";
            var cachedBooks = await _cacheService.GetFromCacheAsync<IEnumerable<Book>>(key);

            if (cachedBooks != null)
            {
                return Ok(cachedBooks);
            }

            var books = await _context.Books.ToListAsync();

            if (books.Any())
            {
                await _cacheService.SetCacheAsync(key, books);
            }

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var key = $"Book:{id}";
            var cachedBook = await _cacheService.GetFromCacheAsync<Book>(key);

            if (cachedBook != null)
            {
                return Ok(cachedBook);
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _cacheService.SetCacheAsync(key, book);

            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var key = $"Book:{id}";
                await _cacheService.SetCacheAsync(key, book);

                key = "Books:All";
                await _cacheService.RemoveCacheAsync(key);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var key = $"Book:{book.Id}";
            await _cacheService.SetCacheAsync(key, book);

            key = "Books:All";
            await _cacheService.RemoveCacheAsync(key);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            var key = $"Book:{id}";
            await _cacheService.RemoveCacheAsync(key);

            key = "Books:All";
            await _cacheService.RemoveCacheAsync(key);

            return NoContent();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}