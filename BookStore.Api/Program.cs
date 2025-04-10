using BookStore.Api.Data;
using BookStore.Api.Endpoints;
using BookStore.Api.Interfaces;
using BookStore.Api.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<BookStoreContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BookStoreContext")
        ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.");
    var serverVersion = ServerVersion.AutoDetect(connectionString);

    options.UseMySql(connectionString, serverVersion);

    if (builder.Environment.IsDevelopment())
    {
        options
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});
builder.Services.AddSingleton<IBookRepository, InMemoryBooksRepository>();

var app = builder.Build();
app.Services.InitializeDb();
app.MapBooksEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
    app.MapSeedsEndpoints();
}

app.UseHttpsRedirection();

app.Run();

