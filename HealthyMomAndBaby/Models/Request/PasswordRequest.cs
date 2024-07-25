namespace HealthyMomAndBaby.Models.Request
{
    public class PasswordRequest
    {
        public string UserName { get; set; }

        public string ConfirmPassword { get; init; }
        public string NewPassword { get; init; }
    }
}
