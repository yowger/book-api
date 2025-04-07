namespace BookStore.Api.Entities;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}