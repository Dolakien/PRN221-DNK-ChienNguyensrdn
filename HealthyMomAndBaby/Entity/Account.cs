
using Newtonsoft.Json;

namespace HealthyMomAndBaby.Entity
{
    public class Account : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
