namespace HealthyMomAndBaby.Entity
{
	public class Product :IEntity
	{
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public int ProductCategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
