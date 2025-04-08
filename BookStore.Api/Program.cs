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

const string GetBookEndpointName = "GetBookById";

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

BookCategory bookCategory = new BookCategory
{
    BookId = book1.Id,
    Book = book1,
    CategoryId = category.Id,
    Category = category
};

book1.BookCategories.Add(bookCategory);

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

app.MapGet("/books", () =>
{
    var bookDtos = books.Select(book => book.ToBookDtoV1()).ToList();

    return bookDtos;
});

app.MapGet("/books/{id}", (Guid id) =>
{
    Book? book = books.FirstOrDefault(book => book.Id == id);

    if (book is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book.ToBookDtoV1());
}).WithName(GetBookEndpointName);

app.MapPost("/books", (CreateBookDtoV1 createBookDto) =>
{
    Book newBook = new Book
    {
        Id = Guid.NewGuid(),
        Title = createBookDto.Title,
        Description = createBookDto.Description,
        PublishedDate = createBookDto.PublishedDate,
        Price = createBookDto.Price,
        AuthorId = createBookDto.AuthorId,
        Author = author
    };

    foreach (var categoryId in createBookDto.CategoryIds)
    {
        newBook.BookCategories.Add(new BookCategory
        {
            BookId = newBook.Id,
            Book = newBook,
            CategoryId = categoryId,
            Category = category
        });
    }

    books.Add(newBook);

    return Results.CreatedAtRoute(
        routeName: GetBookEndpointName,
        routeValues: new { id = newBook.Id },
        value: newBook.ToBookDtoV1()
    );
});

app.Run();

