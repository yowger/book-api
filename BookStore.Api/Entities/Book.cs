namespace BookStore.Api.Entities;
public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public Guid AuthorId { get; set; }
    public required Author Author { get; set; }
    public required decimal Price { get; set; }
    public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

}