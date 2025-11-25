
namespace Wasla_Backend.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating)
        {
            var reviews =await _reviewRepository.GetReviewsByRating(rating);
            return reviews;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByServiceProviderIdAsync(string serviceProviderId)
        {
           var reviews = await _reviewRepository.GetReviewsByServiceProviderIdAsync(serviceProviderId);
            return reviews;
        }
    }
}
