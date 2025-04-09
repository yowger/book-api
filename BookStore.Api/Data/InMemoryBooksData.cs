using BookStore.Api.Entities;

namespace BookStore.Api.Data;

public static class InMemoryBooksData
{
    public static Author Author;
    public static Category Category;
    public static List<Book> Books;

    static InMemoryBooksData()
    {
        Author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "J.K. Rowling"
        };

        Category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Fantasy"
        };

        var book1 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Harry Potter and the Philosopher's Stone",
            Description = "A young wizard embarks on his first year at Hogwarts.",
            PublishedDate = new DateTime(1997, 6, 26),
            Price = 19.99m,
            AuthorId = Author.Id,
            Author = Author
        };

        book1.BookCategories.Add(new BookCategory
        {
            BookId = book1.Id,
            Book = book1,
            CategoryId = Category.Id,
            Category = Category
        });

        var book2 = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Harry Potter and the Chamber of Secrets",
            Description = "Harry Potter returns for his second year at Hogwarts.",
            PublishedDate = new DateTime(1998, 7, 2),
            Price = 20.99m,
            AuthorId = Author.Id,
            Author = Author
        };

        book2.BookCategories.Add(new BookCategory
        {
            BookId = book2.Id,
            Book = book2,
            CategoryId = Category.Id,
            Category = Category
        });

        Books = new List<Book> { book1, book2 };
    }
}
