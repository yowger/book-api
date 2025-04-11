using BookStore.Api.Dtos;
using FluentValidation;

namespace BookStore.Api.validators;

internal sealed class CreateBookDtoValidator : AbstractValidator<CreateBookDtoV1>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title must not be empty.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description must not be empty.")
            .Length(2, 100).WithMessage("Title must be between 2 and 100 characters");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithMessage("Author id must not be empty.");
    }
}