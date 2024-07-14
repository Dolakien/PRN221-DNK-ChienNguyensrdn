namespace HealthyMomAndBaby.Models.Request
{
    public class CreateVoucherRequest
    {
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public string Discount { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
