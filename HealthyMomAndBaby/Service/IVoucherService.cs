using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
    public interface IVoucherService
    {
        Task AddVoucherAsync(CreateVoucherRequest voucher);
        Task UpdateVoucherAsync(Voucher voucher);
        Task DeleteVoucherAsync(int id);
        Task<Voucher?> GetVoucherAsync(int id);
        Task<Voucher?> GetVoucherByCode(string code);
        Task<List<Voucher>> GetAllVouchersAsync();
    }
}
