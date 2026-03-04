namespace Wasla_Backend.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IFileService _fileService;

        public CommentService(
            ICommentRepository commentRepository,
            DateTimeHelper dateTimeHelper,
            IFileService fileService)
        {
            _commentRepository = commentRepository;
            _dateTimeHelper = dateTimeHelper;
            _fileService = fileService;
        }

        public async Task AddComment(AddCommentDto dto)
        {
            var comment = new Comment
            {
                content = dto.content,
                postId = dto.postId,
                userId = dto.userId,
                createdAt = _dateTimeHelper.Now
            };

            if (dto.file != null)
              comment.file = await _fileService.AddFileAsync(dto.file,FileSetting.FilesPosts);

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task UpdateComment(UpdateCommentDto dto)
        {
            var comment = await _commentRepository.GetByIdAsync(dto.commentId);
            if (comment == null)
                throw new NotFoundException(LocalizationKey.CommentNotFound);

            if (dto.content != null)
                comment.content = dto.content;

            comment.file = await _fileService.ReplaceFileAsync(
                comment.file,
                dto.file,
                FileSetting.FilesPosts,
                ReplaceFileMode.ModelNullable);

            comment.createdAt = _dateTimeHelper.Now;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new NotFoundException(LocalizationKey.CommentNotFound);

            _fileService.DeleteFile(comment.file, FileSetting.FilesPosts);

            _commentRepository.Delete(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<GetCommentsResponse>> GetCommentsResponsesByPostId(GetCommentDto dto)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(dto);
        }
    }
}