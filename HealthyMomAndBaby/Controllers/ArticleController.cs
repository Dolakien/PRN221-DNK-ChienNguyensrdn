using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthyMomAndBaby.Controllers
{
    [Route("Article")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: Article
        [HttpGet("blog")]
        public async Task<IActionResult> Index()
        {
            var article = await _articleService.ShowListArticleAsync();
            return View("Article",article);
        }

        // GET: Article/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetDetailArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/Create
        [HttpPost("create")]
        public async Task<IActionResult> Create(ArticleRequest article)
        {
            try
            {
                await _articleService.AddArticleAsync(article);
                return RedirectToAction("Articles", "Admin"); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: Article/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetDetailArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Edit(UpdateArticle article)
        {
            try
            {
                await _articleService.UpdateArticleAsync(article);
                return RedirectToAction("Articles", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // Delete: Article/Delete/5
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id);
                return RedirectToAction("Articles", "Admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
