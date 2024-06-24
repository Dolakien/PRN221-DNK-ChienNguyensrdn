using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
    public interface IArticleService
    {
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
        Task<List<Article?>> ShowListArticleAsync();
        Task<Article?> GetDetailArticleByIdAsync(int id);
    }
}
