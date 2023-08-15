using BookStore.Data;
using BookStore.Entities;

namespace BookStore.DevTest
{
    public class PopulateDatabase
    {
        private readonly DatabaseContext _context;

        public PopulateDatabase(DatabaseContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Books.Any())
            {
                return;
            }

            Book b1 = new("Cem Anos de Solidão", "Gabriel García Márquez", 34.90);
            Book b2 = new("Harry Potter e a Pedra Filosofal", "J.K. Rowling", 29.99);
            Book b3 = new("1984", "George Orwell", 19.80);
            Book b4 = new("Orgulho e Preconceito", "Jane Austen", 24.50);
            Book b5 = new("O Alquimista", "Paulo Coelho", 14.90);
            Book b6 = new("O Assassinato de Roger Ackroyd", "Agatha Christie", 22.00);
            Book b7 = new("Dom Casmurro", "Machado de Assis", 16.90);
            Book b8 = new("A Hora da Estrela", "Clarice Lispector", 12.90);
            Book b9 = new("O Senhor dos Anéis", "J.R.R. Tolkien", 49.90);
            Book b10 = new("O Iluminado", "Stephen King", 39.90);
            Book b11 = new("Kafka à Beira-Mar", "Haruki Murakami", 44.90);
            Book b12 = new("Amada", "Toni Morrison", 26.90);
            Book b13 = new("O Retrato de Dorian Gray", "Oscar Wilde", 18.90);
            Book b14 = new("Ficções", "Jorge Luis Borges", 28.90);
            Book b15 = new("As Aventuras de Tom Sawyer", "Mark Twain", 21.90);
            Book b16 = new("O Velho e o Mar", "Ernest Hemingway", 17.90);
            Book b17 = new("Como Eu Era Antes de Você", "Jojo Moyes", 32.90);
            Book b18 = new("O Código Da Vinci", "Dan Brown", 27.90);
            Book b19 = new("Deuses Americanos", "Neil Gaiman", 42.90);
            Book b20 = new("Americanah", "Chimamanda Ngozi Adichie", 36.90);


            _context.Books.AddRange(
                b1, b2, b3, b4, b5, b6, b7, b8, b9, b10,
                b11, b12, b13, b14, b15, b16, b17, b18, b19, b20
            );

            _context.SaveChanges();
        }
    }
}