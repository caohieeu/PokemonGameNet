using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PokemonGame.Core.Helpers
{
    public class MongoCacheBuilder<T>
    {
        private readonly IDatabase _cache;

        public List<T> Result { get; }
        public string? CacheKeys { get; private set; }
        public List<string> Dependencies { get; } = new();
        public MongoCacheBuilder(List<T> result, IConnectionMultiplexer connection)
        {
            Result = result;
            _cache = connection.GetDatabase();
        }
    }
}
