using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Account> _accountRepository;
        public ArticleService(IRepository<Article> articleRepository, IRepository<Account> accountRepository)
        {
            _articleRepository = articleRepository;
            _accountRepository = accountRepository;
        }
        public async Task AddArticleAsync(ArticleRequest article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }
            var account = await _accountRepository.Get().Where(x => x.Role.RoleName == "ADMIN").FirstAsync();
            var newArticle = new Article
            {
                Title = article.Title,
                Content = article.Content,
                Date = DateTime.Now,
                IsDeleted = false,
                Author = account
            };
            await _articleRepository.AddAsync(newArticle);
            await _articleRepository.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _articleRepository.GetAsync(id);
            if (article == null)
            {
                throw new InvalidOperationException($"Article with id {id} not found.");
            }
            article.IsDeleted = true;
            _articleRepository.Update(article);
            await _articleRepository.SaveChangesAsync();
        }

        public async Task<Article?> GetDetailArticleByIdAsync(int id)
        {
            return await _articleRepository.GetAsync(id);
        }

        public async Task<List<Article?>> ShowListArticleAsync()
        {
            return await _articleRepository.GetValuesAsync();
        }

        public async Task UpdateArticleAsync(UpdateArticle article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            var existingArticle = await _articleRepository.GetAsync(article.Id);
            if (article == null)
            {
                throw new InvalidOperationException($"Order Detail with id {article.Id} not found.");
            }
            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.Date = DateTime.Now;

            _articleRepository.Update(existingArticle);
            await _articleRepository.SaveChangesAsync();
        }
    }
}
