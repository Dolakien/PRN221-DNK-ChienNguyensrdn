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

        public OrdersController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
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
            if(page == "Home")

            return RedirectToAction("Index", "Home");
            else
            {
                return RedirectToAction("Product", "Product");
            }
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

        // POST: Orders/Create
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetDetailOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // PUT: Orders/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // DELETE: Orders/Delete/5
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
