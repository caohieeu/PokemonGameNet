using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Services.IService
{
    public interface IMoveService
    {
        Task<Moves> GetMove(long moveId);
        MoveEffect FilterMove(string effect);
        MoveStateDto TransformMove(Moves movePk);
        PokemonTeamDto ProcessNormalMove(PokemonTeamDto atkPokemon, PokemonTeamDto defPokemon, MoveStateDto moveStateDto);
    }
}
