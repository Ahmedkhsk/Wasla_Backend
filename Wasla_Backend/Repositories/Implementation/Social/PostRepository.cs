namespace Wasla_Backend.Repositories.Implementation
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(Context context) : base(context)
        {
        }


    }
}
