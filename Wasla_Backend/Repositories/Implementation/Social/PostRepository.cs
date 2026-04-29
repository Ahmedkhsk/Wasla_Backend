namespace Wasla_Backend.Repositories.Implementation
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public PostRepository(Context context, IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<Post> GetPostByIdIgnoreQF(int postId)
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.id == postId);
        }

        public async Task<PagedResult<Post>> GetPostsGeneral(PaginationParams paginationParams)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .AsQueryable();

            return await query.ToPagedResultAsync(
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
        }

        public async Task<int> GetPostsCountByUserId(string userId)
        {
            return await _dbSet.CountAsync(p => p.userId == userId);
        }

        public async Task<PagedResult<PostGeneralResponse>> GetPostsByUsingReactionType(GetPostsByUsingReactionTypeDto dto)
        {
            var reactionPostsQuery = _context.Reactions
                .Where(r =>
                    r.userId == dto.userId &&
                    r.targetType == ReactionTargetType.post &&
                    r.reactionType == dto.reactionType)
                .Select(r => r.targetId);

            var postsQuery = _context.Posts
                .Where(p => reactionPostsQuery.Contains(p.id));

            var totalCount = await postsQuery.CountAsync();

            var posts = await postsQuery
                .OrderByDescending(p => p.createdAt)
                .Skip((dto.pageNumber - 1) * dto.pageSize)
                .Take(dto.pageSize)
                .Select(p => new
                {
                    p.id,
                    p.userId,
                    userName = p.user.FullName,
                    profilePhoto = p.user.ProfilePhoto,
                    p.content,
                    p.files,
                    p.createdAt,
                    p.updatedAt,
                    numberofReacts = _context.Reactions.Count(r =>
                        r.targetType == ReactionTargetType.post &&
                        r.targetId == p.id &&
                        r.reactionType == ReactionType.love),
                    numberofSaves = _context.Reactions.Count(r =>
                        r.targetType == ReactionTargetType.post &&
                        r.targetId == p.id &&
                        r.reactionType == ReactionType.save),
                    numberofComments = _context.Comments.Count(c =>
                        c.postId == p.id),
                    isLoved = _context.Reactions.Any(r =>
                        r.userId == dto.userId &&
                        r.targetType == ReactionTargetType.post &&
                        r.targetId == p.id &&
                        r.reactionType == ReactionType.love),
                    isSaved = _context.Reactions.Any(r =>
                        r.userId == dto.userId &&
                        r.targetType == ReactionTargetType.post &&
                        r.targetId == p.id &&
                        r.reactionType == ReactionType.save)
                })
                .ToListAsync();

            var mappedPosts = posts.Select(p => new PostGeneralResponse
            {
                postId = p.id,
                userId = p.userId,
                userName = p.userName,
                content = p.content,
                files = p.files == null
                    ? new List<string>()
                    : p.files.Select(f => _fileUrlBuilderService.GetMediaUrl(f, MediaType.postFile)).ToList(),
                profilePhoto = _fileUrlBuilderService.GetMediaUrl(p.profilePhoto, MediaType.userImage),
                numberofReacts = p.numberofReacts,
                numberofSaves = p.numberofSaves,
                numberofComments = p.numberofComments,
                isLoved = p.isLoved,
                isSaved = p.isSaved,
                createdAt = p.createdAt,
                updatedAt = p.updatedAt
            }).ToList();

            return new PagedResult<PostGeneralResponse>
            {
                Data = mappedPosts,
                PageNumber = dto.pageNumber,
                PageSize = dto.pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Post>> GetPostsByUserId(string userId, int pageNumber, int pageSize)
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