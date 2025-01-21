using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models;

namespace PokemonGame.Dtos.Pokemon
{
    public class TeamPokemonDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Models.Pokemon> Pokemons { get; set; }
    }
}
