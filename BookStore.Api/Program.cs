using BookStore.Api.Data;
using BookStore.Api.Data.seed;
using BookStore.Api.Endpoints;
using BookStore.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDatabaseContext(builder.Configuration, builder.Environment)
    .AddRepositories()
    .AddValidation()
    .AddAuthServices(builder.Configuration, builder.Environment)
    .AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataSeeder.SeedRoles(services);
    await IdentityDataSeeder.SeedAdminUserAsync(services);
}

app.Services.InitializeDb();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
    app.MapSeedsEndpoints();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapAuthEndpoints();
app.MapBooksEndpoints();
app.MapCategoryEndpoints();

app.Run();

