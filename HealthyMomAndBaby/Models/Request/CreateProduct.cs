namespace HealthyMomAndBaby.Models.Request
{
    public class CreateProduct
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
