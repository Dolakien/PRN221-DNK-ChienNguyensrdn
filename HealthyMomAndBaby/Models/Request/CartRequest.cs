namespace HealthyMomAndBaby.Models.Request
{
    public class CartRequest
    {
        public string ProductName { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }

        public double Price { get; set; }

        public double TotoalPrice { get; set; }

        public int Quantity { get; set; }

    }
}
