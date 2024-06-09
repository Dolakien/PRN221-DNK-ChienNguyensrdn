namespace HealthyMomAndBaby.Entity
{
	public class Review :IEntity
	{
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public Account Account { get; set; }
        public Product Product { get; set; }


    }
}
