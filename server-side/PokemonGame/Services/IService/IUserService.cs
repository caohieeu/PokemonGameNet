﻿using MongoDB.Bson;
using PokemonGame.Dtos.Auth;
using PokemonGame.Dtos.Pokemon;
using PokemonGame.Dtos.Response;
using PokemonGame.Models;
using PokemonGame.Models.Response;

namespace PokemonGame.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUser();
        Task<ApplicationUser> GetUser(string username);
        Task<ApplicationUser> GetUserByUsernameAndPassword();
        Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar);
        Task<AuthDto> SignIn(SignInDto user);
        Task<InfoUserResponseDto> GetInfoUser();
        Task<bool> UpdateUser(ApplicationUser user);
        Task<InfoUserResponseDto> GetUserByUsername(string username);
        Task<bool> AddNewTeamPokemon(string userId, AddTeamPokemonDto teamPokemon);
        List<InfoUserResponseDto> GetUsers(string userName);
    }
}
