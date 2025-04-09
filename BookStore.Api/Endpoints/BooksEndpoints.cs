using BookStore.Api.Data;
using BookStore.Api.Entities;
using FluentValidation;

namespace BookStore.Api.Endpoints;
public static class BooksEndpoints
{
    const string GetBookEndpointName = "GetBookById";

    public static RouteGroupBuilder MapBooksEndpoints(this IEndpointRouteBuilder routes)
    {
        var booksGroup = routes.MapGroup("/books");

        booksGroup.MapGet("/", () =>
        {
            var bookDtos = InMemoryBooksData.Books.Select(book => book.ToBookDtoV1()).ToList();

            return bookDtos;
        });

        booksGroup.MapGet("/{id}", (Guid id) =>
        {
            Book? book = InMemoryBooksData.Books.FirstOrDefault(book => book.Id == id);

            if (book is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(book.ToBookDtoV1());
        }).WithName(GetBookEndpointName);

        booksGroup.MapPost("/", (CreateBookDtoV1 createBookDto, IValidator<CreateBookDtoV1> validator) =>
        {
            // for now, validate and throw when global exception handler created.
            var validationResult = validator.Validate(createBookDto);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            Book newBook = new Book
            {
                Id = Guid.NewGuid(),
                Title = createBookDto.Title,
                Description = createBookDto.Description,
                PublishedDate = createBookDto.PublishedDate,
                Price = createBookDto.Price,
                AuthorId = createBookDto.AuthorId,
                Author = InMemoryBooksData.Author
            };

            foreach (var categoryId in createBookDto.CategoryIds)
            {
                newBook.BookCategories.Add(new BookCategory
                {
                    BookId = newBook.Id,
                    Book = newBook,
                    CategoryId = categoryId,
                    Category = InMemoryBooksData.Category
                });
            }

            InMemoryBooksData.Books.Add(newBook);

            return Results.CreatedAtRoute(
                routeName: GetBookEndpointName,
                routeValues: new { id = newBook.Id },
                value: newBook.ToBookDtoV1()
            );
        });

        return booksGroup;
    }
}