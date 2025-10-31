using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Interfaces.Repositories;

namespace PokemonGame.Domain.Repositories
{
    public class RoomBattleRepository : Repository<RoomBattle>, IRoomBattleRepository
    {
        public RoomBattleRepository(IMongoContext context) : base(context)
        {
        }
    }
}
