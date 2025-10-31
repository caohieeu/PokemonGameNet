using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Interfaces.Repositories;

namespace PokemonGame.Domain.Repositories
{
    public class MoveRepository : Repository<Moves>, IMoveRepository
    {
        public MoveRepository(IMongoContext context) : base(context)
        {
        }
    }
}
