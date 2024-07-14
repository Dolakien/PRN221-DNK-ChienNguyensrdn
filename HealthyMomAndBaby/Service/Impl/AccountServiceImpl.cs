using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;

using static NuGet.Packaging.PackagingConstants;


namespace HealthyMomAndBaby.Service.Impl
{
	public class AccountServiceImpl : IAccountService
	{
        private readonly IRepository<Account> _accountRepository;
		private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Point> _pointRepository;
        private readonly IRepository<Order> _orderRepositry;

        public AccountServiceImpl(IRepository<Account> repository,IRepository<Role> roleRepository, IRepository<Point> pointRepository, IRepository<Order> orderRepositry)
		{
			_accountRepository = repository;
            _pointRepository = pointRepository;
            _orderRepositry = orderRepositry;
            _roleRepository = roleRepository;
        }

        public async Task AddAccountAsync(CreateUser account)
		{
            var role = _roleRepository.Get().First(x => x.RoleName == account.RoleName);
            var newUser = new Account
            {
                Email = account.Email,
                UserName = account.UserName,
                Password = account.Password,
                Status = false,
                Role = role
            };
            await _accountRepository.AddAsync(newUser);
			await _accountRepository.SaveChangesAsync();
			
		}

        public async Task DeleteAccountAsync(int id)
		{
            var account = await _accountRepository.GetAsync(id);
            if (account == null)
            {
                throw new InvalidOperationException($"Account with id {id} not found.");
            }

            account.Status = true;
            _accountRepository.Update(account);  
            await _accountRepository.SaveChangesAsync();
        }

        public async Task<Account?> GetDetailProductAsync(int id)
        {
            return await _accountRepository.GetAsync(id);
        }

        public async Task<Account?> Login(string username, string password)
        {
            // Tìm tài khoản với tên người dùng
            var account = _accountRepository.Get().Include(x => x.Role).FirstOrDefault(a => a.UserName == username);

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
            var role = _roleRepository.Get().First(x => x.Id == 1);
            try
            {
                var account = new Account
                {
                    UserName = username,
                    Password = password, // Storing password directly
                    Email = email,
                    Role = role
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

        public async Task<List<Account>> GetAllUser()
        {
            return await _accountRepository.Get().Include(x => x.Role).ToListAsync();
        }

        public async Task UpdateAccountAsync(UpdateAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var existingAccount = await _accountRepository.GetAsync(account.Id);
            if (existingAccount == null)
            {
                throw new InvalidOperationException($"Account with id {account.Id} not found.");
            }
            var role = _roleRepository.Get().First(x => x.RoleName == account.RoleName);
            existingAccount.UserName = account.UserName;
            existingAccount.Password = account.Password;
            existingAccount.Email = account.Email;
            existingAccount.Status = account.Status;
            existingAccount.Role = role;

            _accountRepository.Update(existingAccount);  // Update method is synchronous
            await _accountRepository.SaveChangesAsync();
        }

        //public async Task AddPointAsync(int accountId)
        //{
        //    var user = await _accountRepository.GetAsync(accountId);
        //    if (user == null)
        //    {
        //        throw new InvalidOperationException($"Account with id {accountId} not found.");
        //    }

        //    var userOrders = await _orderRepositry.GetListByIdAsync(o => o.UserId == user.Id);
        //    if (userOrders == null)
        //    {
        //        throw new InvalidOperationException($"Account with id {accountId} not have any order.");
        //    }

        //    int totalPoints = 0;
        //    foreach (var orders in userOrders)
        //    {
        //        totalPoints += 1;
        //    }

        //    var userPoints = await _pointRepository.GetAsync(user.Id);
        //    if (userPoints == null)
        //    {
        //        userPoints = new Point
        //        {
        //            UserId = user.Id,
        //            Points = totalPoints
        //        };
        //        await _pointRepository.AddAsync(userPoints);
        //        await _pointRepository.SaveChangesAsync();

        //    }
        //    else
        //    {
        //        userPoints.Points += totalPoints;
        //        _pointRepository.Update(userPoints);
        //        await _pointRepository.SaveChangesAsync();
        //    }
        //}

        public async Task<List<Point>> ShowListPointAsync()
        {
            return await _pointRepository.GetValuesAsync();
        }

        public async Task<Point?> GetDetailPointAsync(int id)
        {
            return await _pointRepository.GetAsync(id);
        }

        public async Task UpdatePointAsync(Point point)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            var existingPoint = await _pointRepository.GetAsync(point.Id);
            if (existingPoint == null)
            {
                throw new InvalidOperationException($"Point with id {point.Id} not found.");
            }

            existingPoint.UserId = point.UserId;
            existingPoint.Points = point.Points;

            _pointRepository.Update(existingPoint);  // Update method is synchronous
            await _pointRepository.SaveChangesAsync();
        }

        public async Task DeletePointAsync(int id)
        {
            var point = await _pointRepository.GetAsync(id);
            if (point == null)
            {
                throw new InvalidOperationException($"Point with id {id} not found.");
            }

            _pointRepository.Delete(point);
            await _pointRepository.SaveChangesAsync();
        }
    }
}

