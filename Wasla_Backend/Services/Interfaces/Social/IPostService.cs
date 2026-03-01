namespace Wasla_Backend.Services.Interfaces
{
    public interface IPostService
    {
        public Task AddPost(AddPostDto dto);
        public Task UpdatePost(UpdatePostDto dto);
        public Task DeletePost(int postId);
        public Task<PagedResult<PostGeneralResponse>> GetPostsGeneral(string currentUserId, int pageNumber, int pageSize);
        public Task<PostByUserIdResponse> GetPostsByUserId(string userId, string currentUserId, int pageNumber, int pageSize);

    }
}
