using PokemonGame.DAL;
using PokemonGame.Models;
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
