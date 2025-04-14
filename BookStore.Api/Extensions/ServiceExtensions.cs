using System.Text;
using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
using BookStore.Api.Repositories;
using BookStore.Api.Service;
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

    public static IServiceCollection AddTokenService(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddIdentity<AppUser, IdentityRole>()
           .AddEntityFrameworkStores<BookStoreContext>()
           .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(jwtOptions =>
            {
                var signInKey = configuration["Jwt:SigningKey"];

                if (string.IsNullOrEmpty(signInKey))
                {
                    throw new ArgumentException("Missing Jwt:SigningKey");
                }

                jwtOptions.Authority = configuration["Jwt:Issuer"];
                jwtOptions.Audience = configuration["Jwt:Audience"];
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = configuration.GetSection("Jwt:ValidIssuers").Get<string[]>(),
                    ValidAudiences = configuration.GetSection("Jwt:Audience").Get<string[]>(),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(signInKey)
                    )
                };

                jwtOptions.MapInboundClaims = true;

                if (environment.IsDevelopment())
                {
                    jwtOptions.RequireHttpsMetadata = false;
                }
            });

        services.AddAuthorization();

        return services;
    }
}