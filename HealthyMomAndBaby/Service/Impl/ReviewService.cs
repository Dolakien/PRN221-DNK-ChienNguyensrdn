using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepository;
        public ReviewService(IRepository<Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetAsync(id);
            if (review == null)
            {
                throw new InvalidOperationException($"Review with id {id} not found.");
            }

            _reviewRepository.Delete(review);  // Update method is synchronous
            await _reviewRepository.SaveChangesAsync();
        }

        public async Task<Review?> GetDetailReviewByIdAsync(int id)
        {
            return await _reviewRepository.GetAsync(id);
        }

        public async Task<List<Review?>> ShowListReviewAsync()
        {
            return await _reviewRepository.GetValuesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            var existingReview = await _reviewRepository.GetAsync(review.Id);
            if (existingReview == null)
            {
                throw new InvalidOperationException($"Review with id {review.Id} not found.");
            }

            existingReview.AccountId = review.AccountId;
            existingReview.ProductId = review.ProductId;
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.ReviewDate = review.ReviewDate;

            _reviewRepository.Update(existingReview);  // Update method is synchronous
            await _reviewRepository.SaveChangesAsync();
        }
    }
}
