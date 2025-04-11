namespace BookStore.Api.Dtos;

public record BookDtoV1
(
    Guid Id,
    string Title,
    string Description,
    DateTime? PublishedDate,
    decimal Price,
    string AuthorName,
    List<string> Categories
);

public record CreateBookDtoV1
(
    string Title,
    string Description,
    DateTime PublishedDate,
    decimal Price,
    Guid AuthorId,
    List<Guid> CategoryIds
);

public record UpdateBookDtoV1
(
    string Title,
    string Description,
    DateTime PublishedDate,
    decimal Price,
    Guid AuthorId,
    List<Guid> CategoryIds
);
