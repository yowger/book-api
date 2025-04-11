using BookStore.Api.Dtos;
using FluentValidation;

namespace BookStore.Api.validators;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDtoV1>
{
    public UpdateCategoryDtoValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name must not be empty.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}