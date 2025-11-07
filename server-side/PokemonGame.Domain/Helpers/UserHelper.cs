using Microsoft.AspNetCore.Identity;
using PokemonGame.Core.Models.Entities;

namespace PokemonGame.Domain.Helpers
{
    public class UserHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }   
        public HashSet<string> GetRolesName(ApplicationUser applicationUser)
        {
            return _userManager.GetRolesAsync(applicationUser).Result.ToHashSet();
        }
    }
}
