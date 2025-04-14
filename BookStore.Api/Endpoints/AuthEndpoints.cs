using BookStore.Api.Dtos;
using BookStore.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var authGroup = routes.MapGroup("/auth");

            authGroup.MapPost("/register", async
            (
                [FromBody] RegisterDtoV1 registerDto,
                [FromServices] UserManager<AppUser> userManager
            ) =>
            {
                var user = new AppUser { UserName = registerDto.Email, Email = registerDto.Email };
                var result = await userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }

                await userManager.AddToRoleAsync(user, "User");

                return Results.Created("/auth/register", user);
            });
        }
    }
}