namespace HealthyMomAndBaby.Models.Request
{
    public class CreateUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
