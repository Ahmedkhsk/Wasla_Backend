namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        public Task<Post> GetPostByIdIgnoreQF(int postId);
        public Task<PagedResult<Post>> GetPostsGeneral(PaginationParams paginationParams);
        public Task<PagedResult<Post>> GetPostsByUserId(string userId, int pageNumber, int pageSize);
        public Task<PagedResult<PostGeneralResponse>> GetPostsByUsingReactionType(GetPostsByUsingReactionTypeDto dto);
        public Task<int> GetPostsCountByUserId(string userId);
    }
}
