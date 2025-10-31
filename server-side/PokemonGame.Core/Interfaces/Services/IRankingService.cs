using PokemonGame.Core.Models.Dtos.Rank;

namespace PokemonGame.Core.Interfaces.Services
{
    public interface IRankingService
    {
        Task<List<RankDto>> GetRankings();
    }
}
