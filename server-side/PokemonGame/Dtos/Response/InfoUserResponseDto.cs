using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Core.Models;
using PokemonGame.Dtos.Pokemon;

namespace PokemonGame.Dtos.Response
{
    public class InfoUserResponseDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public HashSet<string> Roles { get; set; }
        public List<string> Moves { get; set; } = null;
        public List<TeamPokemonDto> Teams { get; set; } = null;
        public int Point { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
