using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Repositories;

public class EntityFrameworkCategoryRepository : ICategoryRepository
{
    private readonly BookStoreContext _context;

    public EntityFrameworkCategoryRepository(BookStoreContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Select(category => new Category
            {
                Id = category.Id,
                Name = category.Name,
            })
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public Task<Category?> GetByNameAsync(string name)
    {
        var normalizedNAme = name.Trim().ToLower();

        return _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Name == normalizedNAme);
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }


}