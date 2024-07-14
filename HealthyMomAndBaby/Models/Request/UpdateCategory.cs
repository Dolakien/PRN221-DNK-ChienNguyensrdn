namespace HealthyMomAndBaby.Models.Request
{
    public class UpdateCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
    }
}
