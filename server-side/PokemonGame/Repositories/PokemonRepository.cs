using PokemonGame.DAL;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Repositories.IRepository;

namespace PokemonGame.Repositories
{
    public class PokemonRepository : Repository<Pokemon>, IPokemonRepository
    {
        public PokemonRepository(IMongoContext context) : base(context)
        {
        }
    }
}
