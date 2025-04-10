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
builder.Services.AddSingleton<IBookRepository, InMemoryBooksRepository>();
builder.Services.AddDbContext<BookStoreContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BookStoreContext");
      options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

    options.UseMySql(connectionString, serverVersion);

    if (builder.Environment.IsDevelopment())
    {
        options
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});


var app = builder.Build();
app.MapBooksEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

