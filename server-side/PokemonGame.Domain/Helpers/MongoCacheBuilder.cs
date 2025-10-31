using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace PokemonGame.Domain.Helpers
{
     public class MongoCacheBuilder<T>
    {
        public List<T> Result { get; }
        public string? CacheKey { get; private set; }
        public List<string> Dependencies { get; } = new();
        public MongoCacheBuilder(List<T> result)
        {
            Result = result;
        }
        public MongoCacheBuilder<T> Key(string key)
        {
            CacheKey = key;
            return this;
        }
    }
}
