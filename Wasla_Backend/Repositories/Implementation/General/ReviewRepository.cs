

namespace Wasla_Backend.Repositories.Implementation
{
    public class ReviewRepository :  GenericRepository<Reviews>, IReviewRepository
    {
        public ReviewRepository(Context context) : base(context)
        {
        }

        public async Task<float> GetRatingAvgByServiceProvider(string serviceProviderId)
        {

            float? ratingAvg = await _context.Review.Where(r => r.ServiceProviderId == serviceProviderId)
                      .AverageAsync(r => (float?)r.Rating);
            return ratingAvg ?? 0;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating,string serviceProviderId)
        {
            return await _context.Review.Include(r => r.User)
                .AsNoTracking()
                .Where(r =>r.Rating == rating&&r.ServiceProviderId==serviceProviderId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponseDto
                {
                    reviewId = r.Id,
                    userId = r.UserId,  
                    ReviewerName = r.ReviewerName,
                    UserImageUrl = r.User.ProfilePhoto,
                    Rating = r.Rating,
                    Comment = r.Content,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByServiceProviderIdAsync(string serviceProviderId)
        {
            return await _context.Review.Include(r => r.User)
                .AsNoTracking()
                .Where(r => r.ServiceProviderId == serviceProviderId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponseDto
                {
                    reviewId = r.Id,
                    userId = r.UserId,
                    ReviewerName = r.ReviewerName,
                    UserImageUrl = r.User.ProfilePhoto,
                    Rating = r.Rating,
                    Comment = r.Content,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

        }

        public Task<int>CountByServiceProviderAndUserId(string serviceProviderId, string userId)
        {
          return _context.Review.AsNoTracking()
                .Where(r => r.ServiceProviderId == serviceProviderId && r.UserId == userId)
                .CountAsync();
        }
    }
}
