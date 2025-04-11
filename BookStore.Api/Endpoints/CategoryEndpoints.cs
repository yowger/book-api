using BookStore.Api.Dtos;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;

namespace BookStore.Api.Endpoints;

public static class CategoryEndpoints
{
    const string routeName = "categories";
    const string getCategoryByIdEndpointName = "GetCategoryById";

    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var categoriesGroup = routes.MapGroup($"/{routeName}");

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

            return Results.Created($"/{routeName}/{newCategory.Id}", newCategory.ToCategoryDtoV1());
        });

        categoriesGroup.MapGet("/", async (ICategoryRepository categoryRepository) =>
        {
            var categories = await categoryRepository.GetAllAsync();

            return Results.Ok(categories);
        });

        categoriesGroup.MapGet("/{id}", async (Guid id, ICategoryRepository categoryRepository) =>
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(category);
        }).WithName(getCategoryByIdEndpointName);


    }
}