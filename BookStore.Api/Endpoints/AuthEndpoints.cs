using BookStore.Api.Dtos;
using BookStore.Api.Entities;
using BookStore.Api.Interfaces;
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

            authGroup.MapPost("/login", async
            (
                [FromBody] LoginDtoV1 loginDto,
                [FromServices] UserManager<AppUser> userManager,
                [FromServices] ITokenService tokenService
            ) =>
            {
                var user = await userManager.FindByEmailAsync(loginDto.Email.ToLower());

                if (user is null)
                {
                    return Results.Unauthorized();
                }

                var comparePassword = await userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!comparePassword)
                {
                    return Results.Unauthorized();
                }

                var token = tokenService.CreateToken(user);

                return Results.Ok(new { token });
            });
        }
    }
}