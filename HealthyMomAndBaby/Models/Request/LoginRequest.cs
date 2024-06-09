using System.ComponentModel.DataAnnotations;

namespace HealthyMomAndBaby.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
