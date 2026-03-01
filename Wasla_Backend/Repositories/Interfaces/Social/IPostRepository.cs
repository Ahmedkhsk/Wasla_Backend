namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        public Task<PagedResult<Post>> GetPostsGeneral(int pageNumber, int pageSize);
        public Task<PagedResult<Post>> GetPostsByUserId(string userId, int pageNumber, int pageSize);


    }
}
