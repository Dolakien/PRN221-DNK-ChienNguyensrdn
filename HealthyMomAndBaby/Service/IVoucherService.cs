using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
    public interface IVoucherService
    {
        Task AddVoucherAsync(Voucher voucher);
        Task UpdateVoucherAsync(Voucher voucher);
        Task DeleteVoucherAsync(int id);
        Task<Voucher?> GetVoucherAsync(int id);
        Task<List<Voucher>> GetAllVouchersAsync();
    }
}
