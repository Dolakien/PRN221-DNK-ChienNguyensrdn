using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Models.Request
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
    }
}
