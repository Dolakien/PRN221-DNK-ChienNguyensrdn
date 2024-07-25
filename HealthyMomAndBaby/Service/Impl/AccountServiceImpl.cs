using HealthyMomAndBaby.Common;
using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using static NuGet.Packaging.PackagingConstants;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace HealthyMomAndBaby.Service.Impl
{
	public class AccountServiceImpl : IAccountService
	{
        private readonly IRepository<Account> _accountRepository;
		private readonly IRepository<Role> _roleRepository;
        private readonly SmtpSettings _smtpsetting;
        private readonly IRepository<Order> _orderRepositry;

        public AccountServiceImpl(IRepository<Account> repository,IRepository<Role> roleRepository,  IRepository<Order> orderRepositry, IOptions<SmtpSettings> smtpSetting)
		{
			_accountRepository = repository;
            _orderRepositry = orderRepositry;
            _roleRepository = roleRepository;
            _smtpsetting = smtpSetting.Value;
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

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.Get().Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAccountByUserNameAsync(string user)
        {
            return await _accountRepository.Get().Where(x => x.UserName == user).FirstOrDefaultAsync();
        }

        public async Task<Account> GetUserById(int id)
        {
            return await _accountRepository.Get().Include(x => x.Role).FirstAsync(x => x.Id == id);
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
                    Point = 0,
                    Role = role,
                    Status = true,              
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




        public async Task<bool> ResetPassword(ResetPasswordRequest resetPassword)
        {
            var account = await GetAccountByUserNameAsync(resetPassword.UserName);
            if (account == null)
            {
                return false;
            }

            if (resetPassword.NewPassword != resetPassword.ConfirmNewPassword)
            {
                return false;
            }

            var passwordRequest = new PasswordRequest
            {
                UserName = resetPassword.UserName,
                NewPassword = resetPassword.NewPassword,
                ConfirmPassword = resetPassword.ConfirmNewPassword
            };

            var isUpdate = await UpdatePassword(passwordRequest);

            await SendResetPasswordEmail(account.Email);

            return isUpdate;
        }



        public async Task<bool> UpdatePassword(PasswordRequest passwordRequest)
        {
            var account = await GetAccountByUserNameAsync(passwordRequest.UserName);


            if (account == null)
            {
                return false;
            }
            // check old password
            if (account.UserName == passwordRequest.UserName)
            {
                account.Password = passwordRequest.NewPassword;
                await UpdateAccountPassword(account);
                await _accountRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task UpdateAccountPassword(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var existingAccount = await GetAccountByUserNameAsync(account.UserName);
            if (existingAccount == null)
            {
                throw new InvalidOperationException($"Account with UserName {account.UserName} not found.");
            }
            _accountRepository.Update(existingAccount);  // Update method is synchronous
            await _accountRepository.SaveChangesAsync();
        }


        public async Task<bool> SendResetPasswordEmail(string email)
        {
            var account = await GetAccountByEmailAsync(email);
            if (account == null)
            {
                return false;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HealthyMomAndBaby System", _smtpsetting.Username));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Reset Your Password";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <p>Dear {account.UserName},</p>
                <p>Your password has been successfully changed.</p>
                <p>If you did not make this change, please contact support immediately.</p>
                <p>Thank you,</p>
            ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
                    client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
                    await client.SendAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    return false;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }


    }
}

