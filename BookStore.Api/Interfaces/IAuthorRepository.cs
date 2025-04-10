using BookStore.Api.Entities;

namespace BookStore.Api.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Guid id);
}