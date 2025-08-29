using AutoMapper;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Dtos.RoomChat;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InfoUserResponseDto, ApplicationUser>().ReverseMap();
            CreateMap<MovesPokemon, Moves>().ReverseMap();
            CreateMap<RoomChatCreateDto, RoomChat>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => new List<Participant>()));
            CreateMap<RoomChat, RoomChatCreateDto>();
            CreateMap<Participant, InfoUserResponseDto>().ReverseMap();
            CreateMap<ParticipantRoomBattleDto, Pokemon>().ReverseMap();
            CreateMap<MoveStateDto, MovesPokemon>()
                .ForMember(dest => dest.Type, otp => otp.Ignore())
                .ReverseMap();
            CreateMap<PokemonTeamDto, Pokemon>()
                .ForMember(dest => dest.Moves, otp => otp.Ignore())
                .ReverseMap();
        }
    }
}
