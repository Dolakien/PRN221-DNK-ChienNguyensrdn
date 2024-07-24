using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public AccountController(IAccountService accountService, IAuthenticationSchemeProvider schemeProvider)
        {
            _accountService = accountService;
            _schemeProvider = schemeProvider;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _accountService.Login(username, password);

            if (account != null)
            {
                // Đăng nhập thành công, bạn có thể thực hiện các hành động sau đây, chẳng hạn như đặt các biến phiên làm việc, chuyển hướng, v.v.
                // Ví dụ:
                string accountJson = JsonConvert.SerializeObject(account);

                // Lưu chuỗi JSON vào Session
                HttpContext.Session.SetString("LoggedInAccount", accountJson);
                // return RedirectToAction("Dashboard", "Home");    
                if(account.Role.RoleName == "ADMIN")
                {
                    return RedirectToAction("Admin", "Admin"); // Chuyển hướng đến trang chính\

                }else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                // Đăng nhập thất bại, bạn có thể hiển thị thông báo lỗi hoặc chuyển hướng đến trang đăng nhập lại, v.v.
                // Ví dụ:
                // ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View("Login"); // Hiển thị lại trang đăng nhập
            }
        }
        [HttpGet("SignUp")]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup( SignUpRequest model)
        {
            try {
                await _accountService.Register(model.Username, model.Password, model.Email);
                return RedirectToAction("Index", "Home" ,model); // Show the signup page again with validation errors
            } catch (Exception e)
            {
                return View("SignUp");
            }
            
        }
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return RedirectToAction("Dashboard","Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetAccountDetails(int id)
        {
            var account = await _accountService.GetDetailProductAsync(id);
            if (account == null)
            {
                return NotFound(new { message = $"Account with id {id} not found" });
            }
            return Ok(account);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAccount(UpdateAccount account)
        {
            try
            {
                await _accountService.UpdateAccountAsync(account);
                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUser account)
        {

            try
            {
                await _accountService.AddAccountAsync(account);
                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {

                return RedirectToAction(nameof(Login));
            }

            var info = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (info?.Principal == null)
            {

                return RedirectToAction(nameof(Login));
            }

            // Lấy email từ claims
            var emailClaim = info.Principal.FindFirst(ClaimTypes.Email) ?? info.Principal.FindFirst("email");
            var email = emailClaim?.Value;

            var nameClaim = info.Principal.FindFirst(ClaimTypes.Name) ?? info.Principal.FindFirst("name");
            var name = nameClaim?.Value;

            var password = " ";

            if (email != null && name != null && password != null)
            {
                var account1 = await _accountService.GetAccountByEmailAsync(email);
                if(account1 == null) {
                    await _accountService.Register(name, password, email); 
                }

                var account = await _accountService.Login(name, password);

                if (account != null)
                {
                    // Đăng nhập thành công, lưu chuỗi JSON vào Session
                    string accountJson = JsonConvert.SerializeObject(account);
                    HttpContext.Session.SetString("LoggedInAccount", accountJson);

                    // Chuyển hướng dựa trên vai trò của tài khoản
                    if (account.Role.RoleName == "ADMIN")
                    {
                        return RedirectToAction("Admin", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }

            }
            return LocalRedirect(returnUrl ?? "/");

        }
    }
}

