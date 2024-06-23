using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
    public class VoucherRepository : IVoucherService
    {
        private readonly IRepository<Voucher> _voucherRepository;
        public VoucherRepository(IRepository<Product> voucherRepository)
        {
            _voucherRepository = _voucherRepository;
        }
        public async Task AddVoucherAsync(Voucher voucher)
        {
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }

            await _voucherRepository.AddAsync(voucher);
            await _voucherRepository.SaveChangesAsync();
        }

        public async Task DeleteVoucherAsync(int id)
        {
            var voucher = await _voucherRepository.GetAsync(id);
            if (voucher == null)
            {
                throw new InvalidOperationException($"Voucher with id {id} not found.");
            }

            voucher.IsDeleted = true;
            _voucherRepository.Update(voucher);
            await _voucherRepository.SaveChangesAsync();
        }

        public async Task<List<Voucher>> GetAllVouchersAsync()
        {
            return await _voucherRepository.GetValuesAsync();
        }

        public async Task<Voucher?> GetVoucherAsync(int id)
        {
            return await _voucherRepository.GetAsync(id);
        }

        public async Task UpdateVoucherAsync(Voucher voucher)
        {
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }

            var existingVoucher = await _voucherRepository.GetAsync(voucher.Id);
            if (existingVoucher == null)
            {
                throw new InvalidOperationException($"Voucher with id {voucher.Id} not found.");
            }

            existingVoucher.VoucherName = voucher.VoucherName;
            existingVoucher.VoucherCode = voucher.VoucherCode;
            existingVoucher.Discount = voucher.Discount;
            existingVoucher.ExpiryDate = voucher.ExpiryDate;
            existingVoucher.CreateBy = voucher.CreateBy;
            existingVoucher.CreatedBy = voucher.CreatedBy;
            existingVoucher.IsDeleted = voucher.IsDeleted;

            _voucherRepository.Update(existingVoucher);
            await _voucherRepository.SaveChangesAsync();
        }
    }
}
