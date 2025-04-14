using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using BookStore.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseContext
    (
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        var connectionString = configuration.GetConnectionString("BookStoreContext")
            ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

        services.AddDbContext<BookStoreContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion, mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            });

            if (environment.IsDevelopment())
            {
                options
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });


        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IBookRepository, InMemoryBooksRepository>();
        services.AddScoped<ICategoryRepository, EntityFrameworkCategoryRepository>();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddIdentity

        services.AddAuthorization();

        return services;
    }
}