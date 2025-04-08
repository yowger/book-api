namespace BookStore.Api.Entities;

public static class BookExtensions
{
    public static BookDtoV1 ToBookDtoV1(this Book book)
    {
        return new BookDtoV1
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            PublishedDate = book.PublishedDate,
            Price = book.Price,
            AuthorName = book.Author.Name,
            Categories = book.BookCategories.Select(bookCategory => bookCategory.Category.Name).ToList()
        };
    }
}