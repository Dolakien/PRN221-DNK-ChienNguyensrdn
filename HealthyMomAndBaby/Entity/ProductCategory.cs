using Newtonsoft.Json;

namespace HealthyMomAndBaby.Entity
{
    public class ProductCategory :IEntity
    {
        public int Id { get; set; }
        public string ProductCategoryName { get; set; }
        public bool IsAvailable { get; set; }
        [JsonIgnore]
        public IList<Product> Products { get; set; }
    }
}
