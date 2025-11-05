using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Interfaces.Repositories;
using PokemonGame.Core.Interfaces.Services;

namespace PokemonGame.Domain.Repositories
{
    public class MoveRepository : Repository<Moves>, IMoveRepository
    {
        public MoveRepository(IMongoContext context, ICacheService cacheService) : base(context, cacheService)
        {
        }
    }
}
