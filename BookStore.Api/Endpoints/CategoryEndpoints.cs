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

        categoriesGroup.MapPost("/", async (Category category, ICategoryRepository categoryRepository) =>
        {
            // TODO: add find by name, return conflict

            await categoryRepository.AddAsync(category);

            return Results.Created($"/{routeName}/{category.Id}", category);
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