using BookStore.Api.Dtos;
using FluentValidation;

namespace BookStore.Api.validators;

internal sealed class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDtoV1>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}