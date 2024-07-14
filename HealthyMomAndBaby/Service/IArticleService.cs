using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
    public interface IArticleService
    {
        Task AddArticleAsync(ArticleRequest article);
        Task UpdateArticleAsync(UpdateArticle article);
        Task DeleteArticleAsync(int id);
        Task<List<Article?>> ShowListArticleAsync();
        Task<Article?> GetDetailArticleByIdAsync(int id);
    }
}
