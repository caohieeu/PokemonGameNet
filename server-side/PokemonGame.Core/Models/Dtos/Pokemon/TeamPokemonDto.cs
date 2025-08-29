using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Core.Models.Dtos.Pokemon
{
    public class TeamPokemonDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Entities.Pokemon> Pokemons { get; set; }
    }
}
