using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
    public interface IReviewService
    {
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<List<Review?>> ShowListReviewAsync();
        Task<Review?> GetDetailReviewByIdAsync(int id);

    }
}
