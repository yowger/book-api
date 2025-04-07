namespace BookStore.Api.Entities;

public class BookCategory
{
    public Guid BookId { get; set; }
    public required Book Book { get; set; }
    public Guid CategoryId { get; set; }
    public required Category Category { get; set; }
}