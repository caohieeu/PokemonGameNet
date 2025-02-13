using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models.SubModel;
using PokemonGame.Dtos.RoomBattle;

namespace PokemonGame.Models
{
    public class RoomBattle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("participants")]
        public List<ParticipantRoomBattleDto> Participants { get; set; }
        public string? Winner {  get; set; }
        public string Status { get; set; }
        public List<ActionQueueDto> ActionQueue { get; set; } = new List<ActionQueueDto>();
        public ActionLog ActionLog { get; set; }
        public string CurrentTurn { get; set; }
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
