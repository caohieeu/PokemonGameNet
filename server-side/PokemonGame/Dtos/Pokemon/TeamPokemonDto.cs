using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Dtos.Pokemon
{
    public class TeamPokemonDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Core.Models.Entities.Pokemon> Pokemons { get; set; }
    }
}
