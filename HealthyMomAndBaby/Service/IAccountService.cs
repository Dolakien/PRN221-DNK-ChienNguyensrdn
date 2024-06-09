using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
	public interface IAccountService
	{
		Task AddAccountAsync(Account account);
		Task UpdateAccountAsync(Account account);
		Task DeleteAccountAsync(int id);
        Task<Account?> Login(string username, string password);
    }
}
