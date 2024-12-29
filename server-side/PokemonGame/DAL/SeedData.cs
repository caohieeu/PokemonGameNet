using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using PokemonGame.Models;
using PokemonGame.Utils.Global;

namespace PokemonGame.DAL
{
    public class SeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMongoDatabase _mongoDatabase;
        public SeedData(
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IMongoDatabase mongoDatabase)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mongoDatabase = mongoDatabase;
        }
        public async Task SeedingData()
        {
            var userCollection = _mongoDatabase.GetCollection<ApplicationUser>("AspNetUsers");

            if (!userCollection.Find(x => x.UserName == "admin").Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "0123456789",
                };

                var result = await _userManager.CreateAsync(user, "123123@Admin");
                if (!result.Succeeded)
                {
                    return;
                }

                if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.Admin });
                }
                if (!await _roleManager.RoleExistsAsync(Roles.Player))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.Player });
                }

                await _userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }
    }
}
