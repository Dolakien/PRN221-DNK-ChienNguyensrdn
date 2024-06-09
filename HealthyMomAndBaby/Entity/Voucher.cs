namespace HealthyMomAndBaby.Entity
{
	public class Voucher : IEntity
	{
        public int Id { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public double Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CreateBy { get; set; }
        public Account CreatedBy { get; set; }

    }
}
