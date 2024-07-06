using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;


namespace HealthyMomAndBaby.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {

        private readonly IAccountService _accountService;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        public AdminController(IAccountService accountService, IProductService productService, IProductCategoryService productCategoryService)
        {
            _accountService = accountService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Admin()
        {
            var accounts = await _accountService.GetAllUser();
            return View("Admin", accounts);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> Products()
        {
            var products = await _productService.ShowListProductAsync();
            return View("Product", products);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> Categories()
        {
            var categories = await _productCategoryService.GetProductCategories();
            return View("Category", categories);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ShowAccounts()
        {
            var accounts = await _accountService.GetAllUser();
            return View("Admin",accounts);
        }
    }
}
