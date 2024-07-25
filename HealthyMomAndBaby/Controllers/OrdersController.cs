using Microsoft.AspNetCore.Mvc;
using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Service;
using Newtonsoft.Json;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Order")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IVoucherService _voucherService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IAccountService _accountService;

        public OrdersController(IOrderService orderService,
            IProductService productService,
            IVoucherService voucherService,
            IOrderDetailService orderDetailService,
            IAccountService accountService)
        {
            _orderService = orderService;
            _productService = productService;
            _voucherService = voucherService;
            _orderDetailService = orderDetailService;
            _accountService = accountService;
        }

        [HttpGet("Cart")]
        public IActionResult Cart()
        {
            string listCart = HttpContext.Session?.GetString("Cart");
            if (listCart != null)
            {
                var cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(listCart);

                // Bây giờ bạn có thể sử dụng đối tượng loggedInAccount trong action này
                ViewData["Cart"] = cart;
            }

            return View("Cart");
        }

        [HttpGet("Increase/{id}")]
        public async Task<IActionResult> IncreaseItem(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart;
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new HashSet<CartRequest>();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            }
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == id);
            if (existingProduct != null)
            {
                existingProduct.Quantity += 1;
                existingProduct.TotoalPrice = existingProduct.Price * existingProduct.Quantity;
            }
            else
            {
                CartRequest orderDetail = new CartRequest
                {
                    ProductId = id,
                    Image = product.Image,
                    Category = product.ProductCategory.ProductCategoryName,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1,
                    TotoalPrice = product.Price * 1
                };
                cart.Add(orderDetail);
            }
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
            return RedirectToAction("Cart", "Order");
        }

        [HttpGet("Decrease/{id}")]
        public async Task<IActionResult> DecreaseItem(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart;
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new HashSet<CartRequest>();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            }
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == id);
            if (existingProduct != null)
            {
                existingProduct.Quantity -= 1;
                existingProduct.TotoalPrice = existingProduct.Price * existingProduct.Quantity;
            }
            if (existingProduct.Quantity <= 0) {
                cart.Remove(existingProduct);
            }
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
            return RedirectToAction("Cart", "Order");
        }

        [HttpGet("getByUserId")]
        public async Task<IActionResult> OrdersByUserId([FromQuery] int userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);

            return View("History", orders);
        }

        [HttpGet("OrderDetail/{id}")]
        public async Task<IActionResult> OrderDetailById([FromRoute] int id)
		{
			var orders = await _orderDetailService.GetDetailByOrderId(id);

			return View("Detail", orders);
		}

		[HttpGet("AddToCart/{id}")]
        public async Task<IActionResult> AddToCart(int id, int quantity, string page)

        {
            var product = await _productService.GetDetailProductAsync(id);

            // Retrieve the existing cart from the session
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart;

            if (string.IsNullOrEmpty(cartJson))
            {
                // If the cart doesn't exist, create a new one
                cart = new HashSet<CartRequest>();
            }
            else
            {
                // Deserialize the existing cart
                cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            }

            // Check if the product already exists in the cart
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == id);

            if (existingProduct != null)
            {
                // If the product exists, update the quantity
                existingProduct.Quantity += quantity;
                existingProduct.TotoalPrice = existingProduct.Price * existingProduct.Quantity;
            }
            else
            {
                // If the product doesn't exist, add it to the cart
                CartRequest orderDetail = new CartRequest
                {
                    ProductId = id,
                    Image = product.Image,
                    Category = product.ProductCategory.ProductCategoryName,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    TotoalPrice = product.Price * quantity
                };
                cart.Add(orderDetail);
            }

            cartJson = JsonConvert.SerializeObject(cart);

            HttpContext.Session.SetString("Cart", cartJson);
            if (page == "Home")

                return RedirectToAction("Index", "Home");
            else
            {
                return RedirectToAction("GetById", "Product", new { id = id });
            }
        }

        [HttpGet("BuyNow/{id}")]
        public async Task<IActionResult>  BuyNow(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart;
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new HashSet<CartRequest>();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            }
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == id);
            if (existingProduct != null)
            {
                existingProduct.Quantity += 1;
                existingProduct.TotoalPrice = existingProduct.Price * existingProduct.Quantity;
            }
            else
            {
                CartRequest orderDetail = new CartRequest
                {
                    ProductId = id,
                    Image = product.Image,
                    Category = product.ProductCategory.ProductCategoryName,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1,
                    TotoalPrice = product.Price * 1
                };
                cart.Add(orderDetail);
            }
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
            return RedirectToAction("Cart", "Order");
        }

        [HttpGet("deleteCartItem/{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            var existingProduct = cart.FirstOrDefault(c => c.ProductId == id);
            cart.Remove(existingProduct);
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
            return RedirectToAction("Cart", "Order");
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.ShowListOrderAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetDetailOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost("AddCode")]
        public async Task<IActionResult> Create(AddCode addCode)
        {
            if(addCode.Code == null) return RedirectToAction("Cart", "Order");
            var voucher = await _voucherService.GetVoucherByCode(addCode.Code);
            HttpContext.Session.SetString("Discount", voucher?.Discount.ToString() ?? "1");
            return RedirectToAction("Cart", "Order");
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> Create(CheckoutRequest order)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            HashSet<CartRequest> cart = JsonConvert.DeserializeObject<HashSet<CartRequest>>(cartJson);
            order.CartRequests = cart;
            string accountJson = HttpContext.Session?.GetString("LoggedInAccount");
            Account loggedInAccount = JsonConvert.DeserializeObject<Account>(accountJson);
            order.CustomerId = loggedInAccount.Id;
            await _orderService.AddOrderAsync(order);
            cart = [];
            
            string newAccount = JsonConvert.SerializeObject(await _accountService.GetUserById(loggedInAccount.Id));
            HttpContext.Session.SetString("LoggedInAccount", newAccount);
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart",cartJson);
            return RedirectToAction("Index", "Home");
        }

    }
}
