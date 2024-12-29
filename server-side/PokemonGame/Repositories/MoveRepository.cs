using PokemonGame.DAL;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;

namespace PokemonGame.Repositories
{
    public class MoveRepository : Repository<Moves>, IMoveRepository
    {
        public MoveRepository(IMongoContext context) : base(context)
        {
        }
    }
}
