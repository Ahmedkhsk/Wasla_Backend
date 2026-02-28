namespace Wasla_Backend.Services.Interfaces
{
    public interface IPostService
    {
        public Task AddPost(AddPostDto dto);
        public Task UpdatePost(UpdatePostDto dto);
        public Task DeletePost(int postId);
    }
}
