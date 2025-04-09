using BookStore.Api.Endpoints;
using BookStore.Api.Interfaces;
using BookStore.Api.Repositories;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IBookRepository, InMemoryBooksRepository>();

var app = builder.Build();
app.MapBooksEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

