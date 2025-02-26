using PokemonGame.Dtos.Rank;
using PokemonGame.Services.IService;

namespace PokemonGame.Services
{
    public class RankingService : IRankingService
    {
        private readonly IUserService _userService;
        public RankingService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<List<RankDto>> GetRankings()
        {
            var users = await _userService.GetAllUser();

            var usersOrdered = users.OrderByDescending(u => u.Point).ToList();

            var ranking = usersOrdered.Select((user, index) => new RankDto
            {
                UserName = user.UserName,
                Point = user.Point,
                Rank = index + 1,
            }).ToList();

            return ranking;
        }
    }
}
