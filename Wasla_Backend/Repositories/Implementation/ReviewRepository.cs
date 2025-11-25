

namespace Wasla_Backend.Repositories.Implementation
{
    public class ReviewRepository :  GenericRepository<Reviews>, IReviewRepository
    {
        public ReviewRepository(Context context) : base(context)
        {
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByRating(int rating)
        {
            return await _context.Review.Include(r => r.User)
                .AsNoTracking()
                .Where(r =>r.Rating == rating)
                .Select(r => new ReviewResponseDto
                {
                    Id = r.Id,
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
                .Select(r => new ReviewResponseDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    UserImageUrl = r.User.ProfilePhoto,
                    Rating = r.Rating,
                    Comment = r.Content,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

        }
    }
}
