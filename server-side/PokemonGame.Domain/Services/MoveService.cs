using MongoDB.Driver;
using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Interfaces.Repositories;
using PokemonGame.Core.Interfaces.Services;
using PokemonGame.Core.Helpers;
using System.Text.RegularExpressions;
using PokemonGame.Core.Enum;
using PokemonGame.Domain.Exceptions;

namespace PokemonGame.Domain.Services
{
    public class MoveService : IMoveService
    {
        private readonly IMoveRepository _moveRepository;
        public static readonly (string pattern, CategoryMove effectType)[] EffectPatterns = new(string, CategoryMove) [] {
            (@"Has a (\d+)% chance to (burn|paralyze|poison|freeze) the target\.", CategoryMove.StatusEffect),
            (@"Inflicts regular damage\.", CategoryMove.NormalDamage),
            (@"Raises the user's (\w+ \w+) by (\d+) stages\.", CategoryMove.StatBoost),
            (@"Hits (\d+)-(\d+) times in one turn\.", CategoryMove.Multihit),
            (@"Lowers the target's (\w+ \w+) by (\d+) stages\.", CategoryMove.StatDebuff)
            //(@"Inflicts damage equal to the target's max HP.",  ),
        };
        public MoveService(IMoveRepository moveRepository)
        {
            _moveRepository = moveRepository;
        }
        public MoveEffect FilterMove(string effect)
        {
            if (string.IsNullOrEmpty(effect))
                return null;

            var moveEffect = new MoveEffect();

            foreach (var (pattern, effectType) in EffectPatterns)
            {
                var match = Regex.Match(effect, pattern);
                if (match.Success)
                {
                    switch (effectType)
                    {
                        case CategoryMove.StatusEffect:
                            if (double.TryParse(match.Groups[1].Value, out double chance))
                            {
                                moveEffect.Effect = match.Groups[2].Value;
                                moveEffect.EffectChance = chance;
                            }
                            break;
                        case CategoryMove.Multihit:
                        case CategoryMove.NormalDamage:
                            moveEffect.Effect = Enum.GetName(typeof(CategoryMove), effectType);
                            break;
                    }
                    return moveEffect;
                }
            }

            return null;
        }
        public MoveStateDto TransformMove(Moves movePk)
        {
            MoveStateDto moveSd = new MoveStateDto();
            if(movePk != null)
            {
                moveSd.Id = movePk.Id;
                moveSd.Name = movePk.Name;
                moveSd.Accuracy = moveSd.Accuracy;
                moveSd.PP = movePk.PP;
                moveSd.Type = movePk.Type;
                moveSd.Power = movePk.Power;
                moveSd.Accuracy = movePk.Accuracy;
                moveSd.Effect = movePk.Effect;
                moveSd.ShortEffect = movePk.ShortEffect;
                moveSd.MoveData = FilterMove(movePk.Effect);
            }

            return moveSd;
        }

        public async Task<Moves> GetMove(long moveId)
        {
            var filter = Builders<Moves>.Filter.Eq(x => x.Id, moveId);

            if(!await _moveRepository.IsExist(filter))
            {
                throw new NotFoundException($"Move {moveId} is not found");
            }

            var move = await _moveRepository.GetByFilter(filter);

            return move;
        }
        double calculateStab(List<string> typePkm, string typeMove)
        {
            foreach(var type in typePkm)
            {
                if (type == typeMove)
                    return 1.5;
            }

            return 1.0;
        }
        public PokemonTeamDto ProcessNormalMove(PokemonTeamDto atkPokemon, PokemonTeamDto defPokemon, MoveStateDto move)
        {
            var damage = CalculateHelper.CalculateDamage(100, move.Power, atkPokemon.Stat.Atk,
                defPokemon.Stat.Defense, calculateStab(atkPokemon.Type, move.Type), 
                TypeEffectivenessHelper.GetEffectiveness(atkPokemon.Type.First(), defPokemon.Type), false);

            defPokemon.Stat.Hp -= damage;

            return defPokemon;
        }
    }
}
