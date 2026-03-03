namespace Wasla_Backend.Repositories.Implementation
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(Context context) : base(context)
        {
            
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
