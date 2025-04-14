namespace BookStore.Api.Dtos;

public record RegisterDtoV1
(
    string Email,
    string Password
);

public record LoginDtoV1
(
    string Email,
    string Password
);