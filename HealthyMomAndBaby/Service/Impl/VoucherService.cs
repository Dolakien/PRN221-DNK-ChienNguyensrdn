using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service.Impl
{
    public class VoucherService : IVoucherService
    {
        private readonly IRepository<Voucher> _voucherRepository;
        private readonly IRepository<Account> _accountRepository;
        public VoucherService(IRepository<Voucher> voucherRepository, IRepository<Account> accountRepository)
        {
            _voucherRepository = voucherRepository;
            _accountRepository = accountRepository;
        }
        public async Task AddVoucherAsync(CreateVoucherRequest voucher)
        {
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }
			var account = await _accountRepository.Get().Where(x => x.UserName == "admin").FirstAsync();
			var newVoucher = new Voucher
            {
                Discount = Double.Parse(voucher.Discount),
                VoucherName = voucher.VoucherName,
                VoucherCode = voucher.VoucherCode,
                ExpiryDate = DateTime.Now.AddDays(10),
                IsDeleted = false,
                CreatedBy = account
            };

            await _voucherRepository.AddAsync(newVoucher);
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
            return await _voucherRepository.Get().ToListAsync();
        }

        public async Task<Voucher?> GetVoucherAsync(int id)
        {
            return await _voucherRepository.GetAsync(id);
        }

		public async Task<Voucher?> GetVoucherByCode(string code)
		{
			return await _voucherRepository.Get().Where(x=> x.VoucherCode == code).FirstAsync();
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
