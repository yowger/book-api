using BookStore.Api.Dtos;

namespace BookStore.Api.Entities;

public static class CategoryExtensions
{
    public static CategoryDtoV1 ToCategoryDtoV1(this Category category)
    {
        return new CategoryDtoV1(category.Id, category.Name);
    }
}