using System.ComponentModel.DataAnnotations;

namespace HealthyMomAndBaby.Models.Request
{
    public class SignUpRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
