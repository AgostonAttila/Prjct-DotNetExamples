using REPR.Models;
using System.Runtime.InteropServices.Marshalling;

namespace REPR.Repository
{
    public class FakeDb
    {

        List<Book> _books = new List<Book>
        {
         new Book( 1,"egyes" ,1),
         new Book( 2,"kettes",2),
         new Book( 3,"harmas",3)
        };


        public async Task<Book> GetBookById(int id)
        {
            if (_books.Any(p => p.id == id))
                return _books[id];

            return null;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return _books;
        }

        public async Task<Book> CreateBook(string Name)
        {
            _books.Add(new Book(_books.Count, Name, _books.Count));

            return _books.Last();
        }

        public void Delete(int id)
        {
            if (_books.Any(p => p.id == id))
                _books.RemoveAt(id);
        }

    }
}
