using PokemonGame.DAL;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;

namespace PokemonGame.Repositories
{
    public class RoomChatRepository : Repository<RoomChat>, IRoomChatRepository
    {
        public RoomChatRepository(IMongoContext context) : base(context)
        {
        }
    }
}
