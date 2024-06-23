using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
	public interface IAccountService
	{
		Task AddAccountAsync(Account account);
		Task UpdateAccountAsync(Account account);
		Task DeleteAccountAsync(int id);
        Task<Account?> Login(string username, string password);
        Task  Register(string username, string password, string email);
        Task<List<Account>> ShowListProductAsync();
        Task<Account?> GetDetailProductAsync(int id);
    }
}
