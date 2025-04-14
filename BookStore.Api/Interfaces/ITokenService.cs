using BookStore.Api.Entities;

namespace BookStore.Api.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}