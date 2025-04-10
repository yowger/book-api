using BookStore.Api.Entities;
using BookStore.Api.Interfaces;

namespace BookStore.Api.Repositories;

public class EntityFrameworkGamesRepository : IBookRepository
{
    public void Add(Book book)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Book> GetAll()
    {
        throw new NotImplementedException();
    }

    public Book? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(Book book)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

}