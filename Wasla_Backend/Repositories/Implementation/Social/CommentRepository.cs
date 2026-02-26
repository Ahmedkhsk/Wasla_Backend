namespace Wasla_Backend.Repositories.Implementation
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(Context context) : base(context)
        {
            
        }
    }
}
