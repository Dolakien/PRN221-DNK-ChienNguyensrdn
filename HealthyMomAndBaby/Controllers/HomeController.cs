using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HealthyMomAndBaby.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IAccountService accountService, IProductService productService)
        {
            _logger = logger;
            _accountService = accountService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            string accountJson = HttpContext.Session?.GetString("LoggedInAccount");
            if (accountJson != null)
            {
                Account loggedInAccount = JsonConvert.DeserializeObject<Account>(accountJson);

                // Bây giờ bạn có thể sử dụng đối tượng loggedInAccount trong action này
                ViewData["LoggedInAccountId"] = loggedInAccount;
            }
            var products = await _productService.GetProductIsAvailable();
            
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
