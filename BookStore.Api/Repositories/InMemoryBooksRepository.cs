using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;


namespace BookStore.Api.Repositories;

public class InMemoryBooksRepository : IBookRepository
{
    private readonly List<Book> _books = InMemoryBooksData.Books;

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(Guid id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public void Add(Book book)
    {
        _books.Add(book);
    }

    public void Update(Book updatedBook)
    {
        var index = _books.FindIndex(book => book.Id == updatedBook.Id);
        if (index >= 0)
        {
            _books[index] = updatedBook;
        }
    }

    public void Delete(Guid id)
    {
        var book = GetById(id);

        if (book is not null)
        {
            _books.Remove(book);
        }
    }
}