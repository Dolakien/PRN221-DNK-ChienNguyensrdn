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
        private readonly IOrderService _orderService;
        private readonly IArticleService _articleService;
        private readonly IVoucherService _voucherService;
        public AdminController(IAccountService accountService,
            IProductService productService, 
            IProductCategoryService productCategoryService, 
            IOrderService orderService,
            IArticleService articleService,
            IVoucherService voucherService)
        {
            _accountService = accountService;
            _productService = productService;
            _productCategoryService = productCategoryService;
            _orderService = orderService;
            _articleService = articleService;
            _voucherService = voucherService;
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
            var categories = await _productCategoryService.GetProductCategories();
            ViewData["categories"] = categories;
            return View("Product", products);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> Categories()
        {
            var categories = await _productCategoryService.GetProductCategories();
            return View("Category", categories);
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.ShowListOrderAsync();
            return View("Order", orders);
        }

        [HttpGet("Articles")]
        public async Task<IActionResult> Articles()
        {
            var articles = await _articleService.ShowListArticleAsync();
            return View("Article", articles);
        }

        [HttpGet("Vouchers")]
        public async Task<IActionResult> Voucher()
        {
            var vouchers = await _voucherService.GetAllVouchersAsync();
            return View("Voucher", vouchers);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ShowAccounts()
        {
            var accounts = await _accountService.GetAllUser();
            return View("Admin", accounts);
        }
    }
}
