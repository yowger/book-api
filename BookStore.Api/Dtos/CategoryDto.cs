namespace BookStore.Api.Dtos;

public record CategoryDtoV1
(
    Guid Id,
    string Name
);

public record CreateCategoryDtoV1
(
    string Name
);

public record UpdateCategoryDtoV1
(
    string Name
);