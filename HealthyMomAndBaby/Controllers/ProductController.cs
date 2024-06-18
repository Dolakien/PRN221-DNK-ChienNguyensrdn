using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
