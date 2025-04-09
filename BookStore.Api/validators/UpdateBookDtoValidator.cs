using FluentValidation;

namespace BookStore.Api.validators;

public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDtoV1>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description must not be empty.")
            .Length(2, 100).WithMessage("Description must be between 2 and 100 characters.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("Author ID must not be empty.");
        RuleFor(x => x.CategoryIds)
            .NotNull().WithMessage("Category IDs must not be null.")
            .Must(ids => ids.Any()).WithMessage("At least one category ID must be provided.");
    }
}