using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        public ArticleService(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task AddArticleAsync(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            await _articleRepository.AddAsync(article);
            await _articleRepository.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _articleRepository.GetAsync(id);
            if (article == null)
            {
                throw new InvalidOperationException($"Article with id {id} not found.");
            }

            _articleRepository.Delete(article);
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

        public async Task UpdateArticleAsync(Article article)
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
            existingArticle.Date = article.Date;
            existingArticle.AuthorId = article.AuthorId;

            _articleRepository.Update(article);
            await _articleRepository.SaveChangesAsync();
        }
    }
}
