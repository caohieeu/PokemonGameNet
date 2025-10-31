using PokemonGame.Core.Models.Dtos.Response;

namespace PokemonGame.Core.Interfaces.Services
{
    public interface IUserContextService
    {
        bool CheckToken(string token);
        Dictionary<string, string> GetTokenInfo(string token);
        Task<InfoUserResponseDto> GetCurrentUserInfo();
    }
}
