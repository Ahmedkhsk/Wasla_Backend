namespace Wasla_Backend.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly string _filePath;

        public CommentService(ICommentRepository commentRepository, IWebHostEnvironment webHostEnvironment, DateTimeHelper dateTimeHelper)
        {
            _commentRepository = commentRepository;
            _dateTimeHelper = dateTimeHelper;
            _filePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.FilesPosts.TrimStart('/'));
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
            {
                var savedFileName = await FileOperation.SaveFile(dto.file, _filePath);
                comment.file = savedFileName;
            }

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

            string? oldFile = comment.file;
            if (dto.file != null)
            {
                if (oldFile != null)
                     FileOperation.DeleteFile(comment.file, _filePath);

                var newImage = await FileOperation.SaveFile(dto.file, _filePath);
                comment.file = newImage;
            }

            comment.createdAt = _dateTimeHelper.Now;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }
       
        public async Task DeleteComment(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new NotFoundException(LocalizationKey.CommentNotFound);

            if (!string.IsNullOrEmpty(comment.file))
                FileOperation.DeleteFile(comment.file, _filePath);

            _commentRepository.Delete(comment);
            await _commentRepository.SaveChangesAsync();
        }
    }
}
