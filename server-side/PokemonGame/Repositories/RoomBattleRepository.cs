using PokemonGame.DAL;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;

namespace PokemonGame.Repositories
{
    public class RoomBattleRepository : Repository<RoomBattle>, IRoomBattleRepository
    {
        public RoomBattleRepository(IMongoContext context) : base(context)
        {
        }
    }
}
