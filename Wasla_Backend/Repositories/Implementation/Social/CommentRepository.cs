namespace Wasla_Backend.Repositories.Implementation
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public CommentRepository(Context context, IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<Dictionary<int, int>> GetCommentCountsForPosts(List<int> postIds)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => postIds.Contains(c.postId))
                .GroupBy(c => c.postId)
                .Select(g => new { PostId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PostId, x => x.Count);
        }

        public async Task<PagedResult<GetCommentsResponse>> GetCommentsByPostIdAsync(GetCommentDto dto)
        {
            var query = _context.Comments.Where(c => c.postId == dto.postId);
            var totalCount = await query.CountAsync();

            var rawComments = await query
                .OrderByDescending(c => c.createdAt)
                .Skip((dto.pageNumber - 1) * dto.pageSize)
                .Take(dto.pageSize)
                .Select(c => new
                {
                    c.id,
                    c.content,
                    c.file,
                    userName = c.user.FullName,
                    userProfile = c.user.ProfilePhoto,
                    c.createdAt,
                    c.updatedAt,
                    c.userId,
                    numberOfLikes = _context.Reactions.Count(r =>
                        r.targetId == c.id &&
                        r.targetType == ReactionTargetType.comment &&
                        r.reactionType == ReactionType.love),
                    isLove = _context.Reactions.Any(r =>
                        r.targetId == c.id &&
                        r.targetType == ReactionTargetType.comment &&
                        r.reactionType == ReactionType.love &&
                        r.userId == dto.currentUserId)
                })
                .ToListAsync();

            var comments = rawComments.Select(c => new GetCommentsResponse
            {
                commentId = c.id,
                content = c.content,
                file = _fileUrlBuilderService.GetMediaUrl(c.file, MediaType.postFile),
                userName = c.userName,
                userProfile = _fileUrlBuilderService.GetMediaUrl(c.userProfile, MediaType.userImage),
                createdAt = c.createdAt,
                updatedAt = c.updatedAt,
                userId = c.userId,
                numberOfLikes = c.numberOfLikes,
                isLove = c.isLove
            }).ToList();

            return new PagedResult<GetCommentsResponse>
            {
                Data = comments,
                PageNumber = dto.pageNumber,
                PageSize = dto.pageSize,
                TotalCount = totalCount
            };
        }
    }
}