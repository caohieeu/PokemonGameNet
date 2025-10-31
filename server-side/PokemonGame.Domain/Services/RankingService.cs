using PokemonGame.Core.Models.Dtos.Rank;
using PokemonGame.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace PokemonGame.Domain.Services
{
    public class RankingService : IRankingService
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        private readonly string _cacheKey = "_ranking";
        public RankingService(IUserService userService, ICacheService cacheService)
        {
            _userService = userService;
            _cacheService = cacheService;
        }
        public async Task<List<RankDto>> GetRankings()
        {
            var users = await _userService.GetAllUser();

            var usersOrdered = users.OrderByDescending(u => u.Point).ToList();

            var ranking = new List<RankDto>();

            var cachedRanking = await _cacheService.GetCacheAsync<List<RankDto>>(_cacheKey);

            if (cachedRanking == null || !cachedRanking.Any())
            {
                ranking = usersOrdered.Select((user, index) => new RankDto
                {
                    UserName = user.UserName ?? string.Empty,
                    Point = user.Point,
                    Rank = index + 1,
                }).ToList();

                await _cacheService.SetCache(_cacheKey, ranking, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            else
            {
                ranking = cachedRanking;
            }

            return ranking;
        }
    }
}
