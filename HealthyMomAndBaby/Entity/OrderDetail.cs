namespace HealthyMomAndBaby.Entity
{
	public class OrderDetail :IEntity
	{
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public double SubPrice { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
