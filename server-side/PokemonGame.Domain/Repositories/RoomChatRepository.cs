using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Interfaces.Repositories;

namespace PokemonGame.Domain.Repositories
{
    public class RoomChatRepository : Repository<RoomChat>, IRoomChatRepository
    {
        public RoomChatRepository(IMongoContext context) : base(context)
        {
        }
    }
}
