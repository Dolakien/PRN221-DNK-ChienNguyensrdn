namespace HealthyMomAndBaby.Entity
{
	public class Order :IEntity
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public Account User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
