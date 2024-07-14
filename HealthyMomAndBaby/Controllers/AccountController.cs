using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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

    }
}

