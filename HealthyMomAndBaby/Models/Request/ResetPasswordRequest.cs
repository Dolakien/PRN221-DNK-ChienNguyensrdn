namespace HealthyMomAndBaby.Models.Request
{
    public class ResetPasswordRequest
    {
        public string UserName {  get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
