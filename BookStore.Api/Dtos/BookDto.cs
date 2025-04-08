public class BookDtoV1
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime? PublishedDate { get; set; }
    public decimal Price { get; set; }
    public string AuthorName { get; set; } = default!;
    public List<string> Categories { get; set; } = new List<string>();
}

public class CreateBookDtoV1
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime PublishedDate { get; set; }
    public decimal Price { get; set; }
    public Guid AuthorId { get; set; }
    public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}