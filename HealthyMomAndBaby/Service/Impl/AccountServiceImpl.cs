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

        public async Task Register(string username, string password, string email)
        {
            try
            {
                var account = new Account
                {
                    UserName = username,
                    Password = password, // Storing password directly
                    Email = email
                };

                await _accountRepository.AddAsync(account);
                await _accountRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For example, log to a file, database, or console
                Console.WriteLine($"An error occurred while registering the account: {ex.Message}");

                // Optionally, rethrow the exception to be handled at a higher level
                throw;
            }
        }



        public Task UpdateAccountAsync(Account account)
		{
			throw new NotImplementedException();
		}
	}
}
