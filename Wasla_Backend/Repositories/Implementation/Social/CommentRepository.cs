using Wasla_Backend.DTOs.PaginationDTOS;

namespace Wasla_Backend.Repositories.Implementation
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(Context context) : base(context)
        {
            
        }

        public async Task<Dictionary<int, int>> GetCommentCountsForPosts(List<int> postIds)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => postIds.Contains(c.postId))
                .GroupBy(c => c.postId)
                .Select(g => new
                {
                    PostId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.PostId, x => x.Count);
        }

        public async Task<PagedResult<GetCommentsResponse>> GetCommentsByPostIdAsync(GetCommentDto dto)
        {
            var query = _context.Comments
                .Where(c => c.postId == dto.postId);

            var totalCount = await query.CountAsync();

            var comments = await query
                .OrderByDescending(c => c.createdAt)
                .Skip((dto.pageNumber - 1) * dto.pageSize)
                .Take(dto.pageSize)
                .Select(c => new GetCommentsResponse
                {
                    commentId = c.id,
                    content = c.content,
                    file = FileSetting.GetMediaUrl(c.file, MediaType.postFile),

                    userName = c.user.FullName,
                    userProfile = FileSetting.GetMediaUrl(c.user.ProfilePhoto, MediaType.userImage),

                    createdAt = c.createdAt,
                    updatedAt = c.updatedAt,
                    userId = c.userId,

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
