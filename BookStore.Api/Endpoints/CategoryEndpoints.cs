using BookStore.Api.Interfaces;

namespace BookStore.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var categoriesGroup = routes.MapGroup("/categories");

        categoriesGroup.MapGet("/", async (ICategoryRepository categoryRepository) =>
        {
            var categories = await categoryRepository.GetAllAsync();

            return Results.Ok(categories);
        });
    }
}