using BookStore.Api.Data;
using BookStore.Api.Endpoints;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using BookStore.Api.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBookRepository, InMemoryBooksRepository>();
builder.Services.AddDbContext<BookStoreContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BookStoreContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

    options.UseMySql(connectionString, serverVersion);

    if (builder.Environment.IsDevelopment())
    {
        options
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});


var app = builder.Build();
app.Services.InitializeDb();
app.MapBooksEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();

    app.MapPost("/seed", async (BookStoreContext context) =>
    {
        if (await context.Books.AnyAsync())
        {
            return Results.Ok("Database already seeded.");
        }

        var author1 = new Author { Name = "J.K. Rowling" };
        var author2 = new Author { Name = "George R.R. Martin" };
        var author3 = new Author { Name = "J.R.R. Tolkien" };

        context.Authors.Add(author1);
        context.Authors.Add(author2);
        context.Authors.Add(author3);

        var categories = new List<Category>
        {
            new Category {Name = "Fantasy"},
            new Category {Name = "Adventure"},
            new Category {Name = "Mystery"},
            new Category {Name = "Horror"},
            new Category {Name = "Romance"},
        };

        foreach (var category in categories)
        {
            if (!await context.Categories.AnyAsync(c => c.Name == category.Name))
            {
                context.Categories.Add(category);
            }
        }

        var book1 = new Book
        {
            Title = "Harry Potter and the Philosopher's Stone",
            Description = "A young wizard embarks on his first year at Hogwarts.",
            PublishedDate = new DateTime(1997, 6, 26),
            Price = 19.99m,
            Author = author1,
        };

        var book2 = new Book
        {
            Title = "A Game of Thrones",
            Description = "A struggle for power in a world of ice and fire.",
            PublishedDate = new DateTime(1996, 8, 6),
            Price = 29.99m,
            Author = author2,
        };

        var book3 = new Book
        {
            Title = "The Fellowship of the Ring",
            Description = "The first part of the epic journey to destroy the One Ring.",
            PublishedDate = new DateTime(1954, 7, 29),
            Price = 25.99m,
            Author = author3,
        };

        context.Books.Add(book1);
        context.Books.Add(book2);
        context.Books.Add(book3);

        await context.SaveChangesAsync();

        return Results.Ok("Database seeded successfully.");
    });
}

app.UseHttpsRedirection();

app.Run();

