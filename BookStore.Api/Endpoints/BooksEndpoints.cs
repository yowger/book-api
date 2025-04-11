using BookStore.Api.MockData;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using FluentValidation;
using BookStore.Api.Dtos;

namespace BookStore.Api.Endpoints;
public static class BooksEndpoints
{
    const string GetBookEndpointName = "GetBookById";

    public static RouteGroupBuilder MapBooksEndpoints(this IEndpointRouteBuilder routes)
    {
        var booksGroup = routes.MapGroup("/books");

        booksGroup.MapGet("/", (IBookRepository bookRepository) =>
        {
            var bookDtos = bookRepository.GetAll().Select(book => book.ToBookDtoV1()).ToList();

            return Results.Ok(bookDtos);
        });

        booksGroup.MapGet("/{id}", (Guid id, IBookRepository bookRepository) =>
        {
            Book? book = bookRepository.GetById(id);

            if (book is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(book.ToBookDtoV1());
        }).WithName(GetBookEndpointName);

        booksGroup.MapPost("/",
        (
            IBookRepository bookRepository,
            CreateBookDtoV1 createBookDto,
            IValidator<CreateBookDtoV1> validator
        ) =>
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
                var category = InMemoryBooksData.Categories.FirstOrDefault(c => c.Id == categoryId);

                if (category != null)
                {
                    newBook.BookCategories.Add(new BookCategory
                    {
                        BookId = newBook.Id,
                        Book = newBook,
                        CategoryId = category.Id,
                        Category = category
                    });
                }
            }

            bookRepository.Add(newBook);

            return Results.CreatedAtRoute(
                routeName: GetBookEndpointName,
                routeValues: new { id = newBook.Id },
                value: newBook.ToBookDtoV1()
            );
        });

        booksGroup.MapPut("/{id}",
        (
            Guid id,
            IBookRepository bookRepository,
            UpdateBookDtoV1 updateBookDto,
            IValidator<UpdateBookDtoV1> validator
        ) =>
        {
            var validationResult = validator.Validate(updateBookDto);

            // for now, validate and throw when global exception handler created.
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var existingBook = bookRepository.GetById(id);

            if (existingBook is null)
            {
                return Results.NotFound();
            }

            existingBook.Title = updateBookDto.Title;
            existingBook.Description = updateBookDto.Description;
            existingBook.PublishedDate = updateBookDto.PublishedDate;
            existingBook.Price = updateBookDto.Price;
            existingBook.AuthorId = updateBookDto.AuthorId;

            existingBook.BookCategories.Clear();
            foreach (var categoryId in updateBookDto.CategoryIds)
            {
                var category = InMemoryBooksData.Categories.FirstOrDefault(c => c.Id == categoryId);

                if (category != null)
                {
                    existingBook.BookCategories.Add(new BookCategory
                    {
                        BookId = existingBook.Id,
                        Book = existingBook,
                        CategoryId = category.Id,
                        Category = category
                    });
                }
            }

            bookRepository.Update(existingBook);

            return Results.NoContent();
        });

        booksGroup.MapDelete("/{id}", (Guid id, IBookRepository bookRepository) =>
            {
                var book = bookRepository.GetById(id);

                if (book is null)
                {
                    return Results.NotFound();
                }

                bookRepository.Delete(id);

                return Results.NoContent();
            });

        return booksGroup;
    }


}