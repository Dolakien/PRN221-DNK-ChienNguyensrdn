namespace HealthyMomAndBaby.Entity
{
	public class Point : IEntity
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public Account User { get; set; }
    }
}
