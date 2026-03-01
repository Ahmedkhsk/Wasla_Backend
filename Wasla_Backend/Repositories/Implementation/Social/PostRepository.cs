namespace Wasla_Backend.Repositories.Implementation
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(Context context) : base(context)
        {
        }

        public async Task<PagedResult<Post>> GetPostsGeneral(int pageNumber, int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.user);

            var totalCount = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Post>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = posts
            };
        }

        public async Task<PagedResult<Post>> GetPostsByUserId(string userId,int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(p => p.userId == userId)
                .AsNoTracking()
                .Include(p => p.user);

            var totalCount = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Post>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = posts
            };
        }



    }
}
