namespace BookStore.Api.Entities;

public class Author
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
}
