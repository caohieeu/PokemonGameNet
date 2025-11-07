using System.IO.Pipes;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Dtos.RoomChat;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;
using PokemonGame.Domain.Helpers;

namespace PokemonGame.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InfoUserResponseDto, ApplicationUser>();
            CreateMap<ApplicationUser, InfoUserResponseDto>()
                .AfterMap<UserInfoMapping>();
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
        
        public class UserInfoMapping : IMappingAction<ApplicationUser, InfoUserResponseDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            public UserInfoMapping(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }
            public void Process(ApplicationUser source, InfoUserResponseDto destination, ResolutionContext context)
            {
                var userHelper = new UserHelper(_userManager);

                destination.Roles = userHelper.GetRolesName(source);
            }
        }
    }
}
