﻿using AutoMapper;
using PokemonGame.Dtos.Response;
using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Dtos.RoomChat;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Utils.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InfoUserResponseDto, ApplicationUser>().ReverseMap();
            CreateMap<MovesPokemon, Moves>().ReverseMap();
            CreateMap<RoomChatCreateDto, RoomChat>().ReverseMap();
            CreateMap<Participant, InfoUserResponseDto>().ReverseMap();
            CreateMap<ParticipantRoomBattleDto, Pokemon>().ReverseMap();
            CreateMap<MoveStateDto, MovesPokemon>()
                .ForMember(ele => ele.Type, otp => otp.Ignore())
                .ReverseMap();
            CreateMap<PokemonTeamDto, Pokemon>()
                .ForMember(ele => ele.Moves, otp => otp.Ignore())
                .ReverseMap();
        }
    }
}
