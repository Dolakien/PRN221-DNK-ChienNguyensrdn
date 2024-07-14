namespace HealthyMomAndBaby.Models.Request
{
    public class CheckoutRequest
    {
        public string FullName {  get; set; }

        public string ShippAddres { get; set; }

        public string PhoneNumber { get; set; }

        public double TotalPrice { get; set; }

        public int CustomerId { get; set; }

        public HashSet<CartRequest> CartRequests { get; set; }
    }
}
