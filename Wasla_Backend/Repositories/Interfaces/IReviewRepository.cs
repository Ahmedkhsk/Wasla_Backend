
namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Reviews>
    {
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByServiceProviderIdAsync(string serviceProviderId);
        Task<IEnumerable<ReviewResponseDto>>GetReviewsByRating(int rating);
    }
}
