using MongoDB.Driver;
using PokemonGame.Dtos.Auth;
using PokemonGame.Models;

namespace PokemonGame.Repositories.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByFilter(FilterDefinition<ApplicationUser> filter);
        Task<AuthDto> SignIn(SignInDto user);
        Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar);
    }
}
