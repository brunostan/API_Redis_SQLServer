using BookStore.Data;
using BookStore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NuGet.Protocol;
using System.Text.Json;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly DatabaseContext _context;

        public BooksController(IDistributedCache cache, DatabaseContext context)
        {
            _cache = cache;
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            // Redis
            string key = "Books: All"; // chave para armazenar e recuperar a lista de todos os livros
            string? booksString = await _cache.GetStringAsync(key);

            if (booksString != null)
            {
                IEnumerable<Book> _books = JsonSerializer.Deserialize<IEnumerable<Book>>(booksString)!;
                await Console.Out.WriteLineAsync($"\n[Cache Redis] \nKey = \"{key}\" \nData = {_books.ToJson()}\n");

                return Ok(_books);
            }

            //SQL Server
            IEnumerable<Book>? books = await _context.Books.ToListAsync();

            if (books == null)
            {
                return NotFound();
            }

            booksString = JsonSerializer.Serialize(books);
            await _cache.SetStringAsync(key, booksString);

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            // Redis
            string key = $"Book: {id}"; // chave para armazenar e recuperar um livro pelo id
            string? bookString = await _cache.GetStringAsync(key);

            if (bookString != null)
            {
                Book _book = JsonSerializer.Deserialize<Book>(bookString)!;
                await Console.Out.WriteLineAsync($"\n[Cache Redis] \nKey = \"{key}\" \nData = {_book.ToJson()}\n");

                return Ok(_book);
            }

            //SQL Server
            Book? book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            bookString = JsonSerializer.Serialize(book);
            await _cache.SetStringAsync(key, bookString);

            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

                // Redis
                string key = $"Book: {id}"; // chave para encontrar e substituir o livro no cache
                string bookString = JsonSerializer.Serialize(book);
                await _cache.SetStringAsync(key, bookString); // atualiza o livro no cache

                key = "Books: All"; // chave para invalidar a lista de todos os livros no cache
                await _cache.RemoveAsync(key); // remove a lista do cache para que a próxima chamada da função GetBooks() busque os dados atualizados do banco de dados


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DatabaseContext.Books'  is null.");
            }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Redis
            string key = $"Book: {book.Id}"; // chave para salvar o livro no cache
            string bookString = JsonSerializer.Serialize(book);
            await _cache.SetStringAsync(key, bookString); // salva o livro no cache

            key = "Books: All"; // chave para invalidar a lista de todos os livros no cache
            await _cache.RemoveAsync(key); // remove a lista do cache para que a próxima chamada da função GetBooks() busque os dados atualizados do banco de dados

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Redis
            string key = $"Book: {id}"; // chave para encontrar e remover o livro do cache
            await _cache.RemoveAsync(key); // remove o livro do cache

            key = "Books: All"; // chave para invalidar a lista de todos os livros no cache
            await _cache.RemoveAsync(key); // remove a lista do cache para que a próxima chamada da função GetBooks() busque os dados atualizados do banco de dados

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

