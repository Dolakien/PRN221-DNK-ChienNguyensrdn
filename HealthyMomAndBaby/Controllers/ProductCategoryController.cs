using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Category")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCategory category)
        {

            try
            {
                await _productCategoryService.Add(category);
                return RedirectToAction("Categories", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productCategoryService.Delete(id);
                return RedirectToAction("Categories", "Admin"); ;
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateCategory category)
        {

            try
            {
                await _productCategoryService.Update(category);
                return RedirectToAction("Categories", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
    }
}
