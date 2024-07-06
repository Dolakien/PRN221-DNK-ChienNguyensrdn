
using Newtonsoft.Json;

namespace HealthyMomAndBaby.Entity
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        [JsonIgnore]
        public ICollection<Account> Accounts { get; set; }

    }
}
