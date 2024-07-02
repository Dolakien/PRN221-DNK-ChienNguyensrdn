using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost("Login")]
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
        [HttpGet("Signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup( SignUpRequest model)
        {
           
                 await _accountService.Register(model.Username, model.Password, model.Email);

             

            return View(model); // Show the signup page again with validation errors
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Ok(new { message = "Account deleted successfully" });
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
        [HttpGet("list")]
        public async Task<IActionResult> ShowAccounts()
        {
            var accounts = await _accountService.ShowListProductAsync();
            return Ok(accounts);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount([FromBody] Account account)
        {
            try
            {
                await _accountService.UpdateAccountAsync(account);
                return Ok(new { message = "Account updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("point/{id}")]
        public async Task<IActionResult> AddPoint(int id)
        {
            await _accountService.AddPointAsync(id);
            return View(id);
        }


        [HttpGet("point/details/{id}")]
        public async Task<IActionResult> GetPointDetails(int id)
        {
            var point = await _accountService.GetDetailPointAsync(id);
            if (point == null)
            {
                return NotFound(new { message = $"Point with id {id} not found" });
            }
            return Ok(point);
        }


        [HttpGet("point/list")]
        public async Task<IActionResult> ShowPoints()
        {
            var points = await _accountService.ShowListPointAsync();
            return Ok(points);
        }


        [HttpPost("point/update")]
        public async Task<IActionResult> UpdatePoint([FromBody] Point point)
        {
            try
            {
                await _accountService.UpdatePointAsync(point);
                return Ok(new { message = "Point updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost("point/delete/{id}")]
        public async Task<IActionResult> Deletepoint(int id)
        {
            try
            {
                await _accountService.DeletePointAsync(id);
                return Ok(new { message = "Point deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

