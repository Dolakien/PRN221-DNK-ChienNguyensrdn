using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
	public class AccountServiceImpl : IAccountService
	{
		private readonly IRepository<Account> _accountRepository;
		public AccountServiceImpl(IRepository<Account> repository)
		{
			_accountRepository = repository;
		}

		public async Task AddAccountAsync(Account account)
		{
			await _accountRepository.AddAsync(account);
			await _accountRepository.SaveChangesAsync();
			
		}

		public Task DeleteAccountAsync(int id)
		{
			throw new NotImplementedException();
		}

        public async Task<Account?> Login(string username, string password)
        {
            // Tìm tài khoản với tên người dùng
            var accounts = await _accountRepository.GetValuesAsync();
            var account = accounts?.FirstOrDefault(a => a.UserName == username);

            if (account == null)
            {
                // Tài khoản không tồn tại
                return null;
            }

            // Kiểm tra mật khẩu
            if (account.Password == password)
            {
                // Thông tin đăng nhập chính xác
                return account;
            }
            else
            {
                // Mật khẩu không chính xác
                return null;
            }
        }

        public Task UpdateAccountAsync(Account account)
		{
			throw new NotImplementedException();
		}
	}
}
