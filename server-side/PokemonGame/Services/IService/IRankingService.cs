using PokemonGame.Dtos.Rank;

namespace PokemonGame.Services.IService
{
    public interface IRankingService
    {
        Task<List<RankDto>> GetRankings();
    }
}
