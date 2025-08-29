using PokemonGame.Core.Models.Dtos.Rank;

namespace PokemonGame.Services.IService
{
    public interface IRankingService
    {
        Task<List<RankDto>> GetRankings();
    }
}
