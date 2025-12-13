namespace Wasla_Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByServiceProviderIdAsync(string serviceProviderId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating);
        Task AddReviewAsync(AddReviewDto review);
        Task UpdateReview(UpdateReviewDto updatereview);
        Task DeleteReviewAsync(int reviewId);
    }
}
