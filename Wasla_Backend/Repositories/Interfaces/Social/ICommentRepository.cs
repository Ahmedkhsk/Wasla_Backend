using Wasla_Backend.DTOs.PaginationDTOS;

namespace Wasla_Backend.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Task<PagedResult<GetCommentsResponse>> GetCommentsByPostIdAsync(GetCommentDto dto);
        public Task<Dictionary<int, int>> GetCommentCountsForPosts(List<int> postIds);
    }
}
