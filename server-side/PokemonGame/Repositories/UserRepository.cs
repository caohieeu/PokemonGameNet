using PokemonGame.DAL;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;

namespace PokemonGame.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {
        }
    }
}
