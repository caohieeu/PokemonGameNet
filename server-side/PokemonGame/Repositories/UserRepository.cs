using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using PokemonGame.DAL;
using PokemonGame.Dtos.Auth;
using PokemonGame.Exceptions;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PokemonGame.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly AppSetting _appSetting;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMongoContext _context;
        private readonly IConfiguration _config;
        public UserRepository(
            IOptionsMonitor<AppSetting> options,
            IConfiguration config,
            IMongoContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager
            ) : base(context)
        {
            _appSetting = options.CurrentValue;
            _context = context;
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public string GenerateToken(ApplicationUser user, List<Claim> listClaim)
        {

            var authClaims = new List<Claim>
            {
                new Claim("UserName", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var claim in listClaim)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, claim.Value));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthDto> SignIn(SignInDto user)
        {
            AuthDto authResponseDto = new AuthDto
            {
                result = false,
            };

            var userExist = await _userManager.FindByNameAsync(user.username);
            var passwordValid = await _userManager.CheckPasswordAsync(
                userExist, user.password);
            if (userExist == null || !passwordValid)
            {
                throw new NotFoundException("Sai tài khoản hoặc mật khẩu");
            }

            var userRoles = await _userManager.GetRolesAsync(userExist);
            var listClaim = new List<Claim>();
            foreach(var role in userRoles)
            {
                listClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            authResponseDto.result = true;
            authResponseDto.token = GenerateToken(userExist, listClaim);

            return authResponseDto;
        }

        public async Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar)
        {
            string filePath = "";

            if(avatar != null && avatar.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Avatars");
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{Guid.NewGuid()}_{avatar.FileName}";
                filePath = Path.Combine(folderPath, fileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                filePath = $"Images/Avatars/{fileName}";
            }

            var user = new ApplicationUser
            {
                Id = ObjectId.GenerateNewId(),
                UserName = signUpDto.UserName,
                Email = signUpDto.Email,
                DisplayName = signUpDto.DisplayName,
                PhoneNumber = signUpDto.PhoneNumber,
                ImagePath = filePath,
                DateCreated = DateTime.Now, 
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if(result.Succeeded)
            {
                if(!string.IsNullOrEmpty(signUpDto.Role))
                {
                    if(!await _roleManager.RoleExistsAsync(signUpDto.Role))
                    {
                        throw new NotFoundException($"Role {signUpDto.Role} is not found");
                    }
                    await _userManager.AddToRoleAsync(user, signUpDto.Role);
                }
            }
            else
            {
                throw new BadRequestException($"Failed to create account: {JsonSerializer.Serialize(result)}");
            }

            return true;
        }
    }
}
