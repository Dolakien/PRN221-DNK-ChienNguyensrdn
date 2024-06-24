using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        // GET: Review
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.ShowListReviewAsync();
            return View(reviews);
        }

        // GET: Review/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var reviews = await _reviewService.GetDetailReviewByIdAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }
            return View(reviews);
        }


        // POST: Review/Create
        [HttpPost]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.AddReviewAsync(review);
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Review/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _reviewService.GetDetailReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // PUT: Review/Edit/5
        [HttpPut]
        public async Task<IActionResult> Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.UpdateReviewAsync(review);
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }



        // DELETE: Review/Delete/5
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
