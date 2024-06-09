namespace HealthyMomAndBaby.Entity
{
    public class ProductCategory :IEntity
    {
        public int Id { get; set; }
        public string ProductCategoryName { get; set; }
        public IList<Product> Products { get; set; }
    }
}
