using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetDetailProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("Product",product);
        }

        // POST: Product/Add
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProduct product)
        {

            try
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction("Products", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
           
        }

        // POST: Product/Edit/5
        [HttpPost("update")]
        public async Task<IActionResult> Edit(UpdateProduct product)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction("Products", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: Product/Delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction("Products", "Admin"); ;
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}

