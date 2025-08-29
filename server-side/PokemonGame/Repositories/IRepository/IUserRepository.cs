using MongoDB.Driver;
using PokemonGame.Core.Models.Dtos.Auth;
using PokemonGame.Core.Models.Entities;

namespace PokemonGame.Repositories.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByFilter(FilterDefinition<ApplicationUser> filter);
        Task<AuthDto> SignIn(SignInDto user);
        Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar);
        Task<IEnumerable<object>> GetRankings();
    }
}
