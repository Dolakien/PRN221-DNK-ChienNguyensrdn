using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
	public interface IAccountService
	{
		Task AddAccountAsync(CreateUser account);
		Task UpdateAccountAsync(UpdateAccount account);
		Task DeleteAccountAsync(int id);
        Task<Account?> Login(string username, string password);
        Task  Register(string username, string password, string email);
        Task<List<Account>> GetAllUser();
        Task<Account> GetUserById(int id);
        Task<Account?> GetDetailProductAsync(int id);
    }
}
