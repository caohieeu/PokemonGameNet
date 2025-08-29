using System.Security.Cryptography.X509Certificates;

namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class BattleResultDto
    {
        public string Attacker {  get; set; }
        public string Defender { get; set; }
        public long IdMoveUsed { get; set; }
        public int DamageDealt { get; set; }
        public List<string> EffectApplied { get; set; } = new();
        public int Hits {  get; set; }
        public bool DefenderFainted => DamageDealt > 0 && DefenderHP <= 0; 
        public int DefenderHP {  get; set; }
    }
}
