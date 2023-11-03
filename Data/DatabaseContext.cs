using BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
