using PokemonGame.DAL;
using PokemonGame.Core.Models.Entities;
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
