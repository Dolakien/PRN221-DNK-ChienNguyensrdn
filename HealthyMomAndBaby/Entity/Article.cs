namespace HealthyMomAndBaby.Entity
{
	public class Article: IEntity
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int AuthorId { get; set; }
        public bool IsDeleted { get; set; }
        public Account Author { get; set; }
    }
}
