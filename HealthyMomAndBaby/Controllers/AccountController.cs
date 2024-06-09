using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _accountService.Login(username, password);

            if (account != null)
            {
                // Đăng nhập thành công, bạn có thể thực hiện các hành động sau đây, chẳng hạn như đặt các biến phiên làm việc, chuyển hướng, v.v.
                // Ví dụ:
                // HttpContext.Session.SetString("UserId", account.Id.ToString());
                // return RedirectToAction("Dashboard", "Home");
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chính
            }
            else
            {
                // Đăng nhập thất bại, bạn có thể hiển thị thông báo lỗi hoặc chuyển hướng đến trang đăng nhập lại, v.v.
                // Ví dụ:
                // ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View("Index"); // Hiển thị lại trang đăng nhập
            }
        }
    }
}

