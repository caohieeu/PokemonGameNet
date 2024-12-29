using System.ComponentModel.DataAnnotations;

namespace PokemonGame.Dtos.Auth
{
    public class SignUpDto
    {
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
