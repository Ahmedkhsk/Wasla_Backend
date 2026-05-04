using Wasla_Backend.Helpers.MlHelper;

namespace Wasla_Backend.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IPostRepository _postRepository;
        private readonly IGenericRepository<ApplicationUser> _UserRepository;
        private readonly ToxicityClassifier _toxicityClassifier;

        public CommentService(
            ICommentRepository commentRepository,
            DateTimeHelper dateTimeHelper,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IPostRepository postRepository,
            IGenericRepository<ApplicationUser> UserRepository,
            ToxicityClassifier toxicityClassifier

        )
        {
            _commentRepository = commentRepository;
            _dateTimeHelper = dateTimeHelper;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _postRepository = postRepository;
            _UserRepository = UserRepository;
            _toxicityClassifier = toxicityClassifier;
        }

        public async Task AddComment(AddCommentDto dto)
        {
            var post = await _postRepository.GetByIdAsync(dto.postId);

            if (post == null)
                throw new NotFoundException(LocalizationKey.NoPostsFound);
            var resident = await _UserRepository.GetByIdAsync(dto.userId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            if (dto.content != null)
            {
                var isToxic = _toxicityClassifier.IsBadWord(dto.content);
                if (isToxic)
                    throw new BadRequestException(LocalizationKey.CommentContentIsInappropriate);
            }

            var comment = new Comment
            {
                content = dto.content,
                postId = dto.postId,
                userId = dto.userId,
                createdAt = _dateTimeHelper.Now
            };

            if (dto.file != null)
                comment.file = await _fileService.AddFileAsync(
                    dto.file,
                    _fileUrlBuilderService.GetPath(MediaType.postFile)
                );

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
            var metadata = new Dictionary<string, string>
{
    { "UserName", resident.FullName ?? "User" }
};
            var image=_fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage);
            var postcomment=string.Concat(dto.postId," , ",comment.id);

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                post.userId, 
                NotificationType.postCommented,
                postcomment,
                image,
                "en",
                metadata
            ));
        }

        public async Task UpdateComment(UpdateCommentDto dto)
        {
            var comment = await _commentRepository.GetByIdAsync(dto.commentId);
            if (comment == null)
                throw new NotFoundException(LocalizationKey.CommentNotFound);

            if (dto.content != null)
            {
                var isToxic = _toxicityClassifier.IsBadWord(dto.content);
                if (isToxic)
                    throw new BadRequestException(LocalizationKey.CommentContentIsInappropriate);

                comment.content = dto.content;
            }

            comment.file = await _fileService.ReplaceFileAsync(
                comment.file,
                dto.file,
                _fileUrlBuilderService.GetPath(MediaType.postFile)
            );

            comment.createdAt = _dateTimeHelper.Now;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new NotFoundException(LocalizationKey.CommentNotFound);

            _fileService.DeleteFile(comment.file, _fileUrlBuilderService.GetPath(MediaType.postFile));

            _commentRepository.Delete(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<GetCommentsResponse>> GetCommentsResponsesByPostId(GetCommentDto dto)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(dto);
        }
    }
}
