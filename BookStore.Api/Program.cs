using BookStore.Api.Data;
using BookStore.Api.Endpoints;
using BookStore.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDatabaseContext(builder.Configuration, builder.Environment)
    .AddApplicationServices();

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

