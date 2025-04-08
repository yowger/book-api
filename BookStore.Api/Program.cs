using BookStore.Api.Entities;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

Author author = new Author
{
    Id = Guid.NewGuid(),
    Name = "J.K. Rowling"
};

Category category = new Category
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
    AuthorId = author.Id,
    Author = author
};

book1.BookCategories.Add(new BookCategory
{
    BookId = book1.Id,
    Book = book1,
    CategoryId = category.Id,
    Category = category
});

var book2 = new Book
{
    Id = Guid.NewGuid(),
    Title = "Harry Potter and the Chamber of Secrets",
    Description = "Harry Potter returns for his second year at Hogwarts.",
    PublishedDate = new DateTime(1998, 7, 2),
    Price = 20.99m,
    AuthorId = author.Id,
    Author = author
};

book2.BookCategories.Add(new BookCategory
{
    BookId = book2.Id,
    Book = book2,
    CategoryId = category.Id,
    Category = category
});

List<Book> books = new List<Book> { book1, book2 };

app.MapGet("/", () => "Hello World");
app.MapGet("/books", () =>
{
    var bookDtos = books.Select(book => new BookDto
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description,
        PublishedDate = book.PublishedDate,
        Price = book.Price,
        AuthorName = book.Author.Name,
        Categories = book.BookCategories.Select(bookCategory => bookCategory.Category.Name).ToList()
    }).ToList();

    return bookDtos;
});
app.MapGet("/books/{id}", (Guid id) =>
{
    Book? book = books.FirstOrDefault(book => book.Id == id);

    if (book is null)
    {
        return Results.NotFound();
    }

    var bookDto = new BookDto
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description,
        PublishedDate = book.PublishedDate,
        Price = book.Price,
        AuthorName = book.Author.Name,
        Categories = book.BookCategories.Select(bc => bc.Category.Name).ToList()
    };

    return Results.Ok(bookDto);
});

app.Run();

