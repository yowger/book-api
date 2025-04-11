using BookStore.Api.Dtos;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using FluentValidation;

namespace BookStore.Api.Endpoints;

public static class CategoryEndpoints
{
    const string getCategoryByIdEndpointName = "GetCategoryById";

    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var categoriesGroup = routes.MapGroup("categories");

        categoriesGroup.MapPost("/", async
        (
            CreateCategoryDtoV1 categoryDto,
            IValidator<CreateCategoryDtoV1> validator,
            ICategoryRepository categoryRepository
        ) =>
        {
            var validationResult = validator.Validate(categoryDto);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var existingCategory = await categoryRepository.GetByNameAsync(categoryDto.Name);

            if (existingCategory is not null)
            {
                return Results.Conflict($"Category '{categoryDto.Name}' already exists.");
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name
            };

            await categoryRepository.AddAsync(newCategory);

            return Results.CreatedAtRoute(
                routeName: getCategoryByIdEndpointName,
                routeValues: new { id = newCategory.Id },
                value: newCategory.ToCategoryDtoV1()
            );
        });

        categoriesGroup.MapGet("/", async (ICategoryRepository categoryRepository) =>
        {
            var categories = await categoryRepository.GetAllAsync();

            var categoryDtos = categories.Select(category => category.ToCategoryDtoV1()).ToList();

            return Results.Ok(categoryDtos);
        });

        categoriesGroup.MapGet("/{id}", async (Guid id, ICategoryRepository categoryRepository) =>
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(category.ToCategoryDtoV1);
        }).WithName(getCategoryByIdEndpointName);

        categoriesGroup.MapPut("/{id}", async
        (
            Guid id,
            UpdateCategoryDtoV1 categoryDto,
            IValidator<UpdateCategoryDtoV1> validator,
            ICategoryRepository categoryRepository
        ) =>
        {
            var validationResult = validator.Validate(categoryDto);
            
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var existingCategory = await categoryRepository.GetByIdAsync(id);

            if (existingCategory is null)
            {
                return Results.NotFound();
            }

            existingCategory.Name = categoryDto.Name;
            await categoryRepository.UpdateAsync(existingCategory);

            return Results.NoContent();
        });

        categoriesGroup.MapDelete("/{id}", async (Guid id, ICategoryRepository categoryRepository) =>
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return Results.NotFound();
            }

            await categoryRepository.DeleteAsync(id);

            return Results.NoContent();
        });

    }
}