using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Data;

public static class DataExtensions
{
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreContext>();

        dbContext.Database.Migrate();
    }
}