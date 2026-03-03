namespace Wasla_Backend.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Task<PagedResult<GetCommentsResponse>> GetCommentsByPostIdAsync(GetCommentDto dto);
    }
}
