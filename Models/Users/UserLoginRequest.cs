using System.ComponentModel.DataAnnotations;

namespace DisneyChallengeV2.Models.Users
{
    public class UserLoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
