using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Product/Index
        public async Task<IActionResult> Index()
        {
            var products = await _productService.ShowListProductAsync();
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Product/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: Product/Add
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}

