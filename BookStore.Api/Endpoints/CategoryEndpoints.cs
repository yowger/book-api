using BookStore.Api.Dtos;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;

namespace BookStore.Api.Endpoints;

public static class CategoryEndpoints
{
    const string getCategoryByIdEndpointName = "GetCategoryById";

    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var categoriesGroup = routes.MapGroup("categories");

        categoriesGroup.MapPost("/", async (CreateCategoryDtoV1 category, ICategoryRepository categoryRepository) =>
        {
            var existingCategory = await categoryRepository.GetByNameAsync(category.Name);

            if (existingCategory is not null)
            {
                return Results.Conflict($"Category '{category.Name}' already exists.");
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = category.Name
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



        categoriesGroup.MapPut("/{id}", async (Guid id, ICategoryRepository categoryRepository) =>
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id);

            if (existingCategory is null)
            {
                return Results.NotFound();
            }

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