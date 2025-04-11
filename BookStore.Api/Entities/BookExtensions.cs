using BookStore.Api.Dtos;

namespace BookStore.Api.Entities;

public static class BookExtensions
{
    public static BookDtoV1 ToBookDtoV1(this Book book)
    {
        return new BookDtoV1(
            book.Id,
            book.Title,
            book.Description,
            book.PublishedDate,
            book.Price,
            book.Author.Name,
            book.BookCategories.Select(bookCategory => bookCategory.Category.Name).ToList()
        );
    }
}
