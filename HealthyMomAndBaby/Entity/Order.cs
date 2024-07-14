namespace HealthyMomAndBaby.Entity
{
	public class Order :IEntity
	{
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public string FullName { get; set; }
        public string ShipAddress { get; set; }
        public double Voucher {  get; set; }
        public string PhoneNumber { get; set; }
        public Account User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
