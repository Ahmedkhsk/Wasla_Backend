namespace Wasla_Backend.Services.Implementation
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionReepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public ReactionService(IReactionRepository reactionReepository,DateTimeHelper dateTimeHelper,IUserRepository userRepository
            , IPostRepository postRepository, IFileUrlBuilderService fileUrlBuilderService
            ) 
        {
            _reactionReepository = reactionReepository;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task ToggleReaction(ToggleReactionDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var existingReaction = await _reactionReepository.GetReaction(dto);
            
            if (existingReaction != null)
                 _reactionReepository.Delete(existingReaction);
            else
            {
                var newReaction = new Reaction
                {
                    targetId = dto.targetId,
                    targetType = dto.targetType,
                    reactionType = dto.reactionType,
                    userId = dto.userId,
                    createdAt = _dateTimeHelper.Now
                };

                await _reactionReepository.AddAsync(newReaction);
                var post = await _postRepository.GetByIdAsync(dto.targetId);

                if (post != null && post.userId != dto.userId)
                {
                    var image=_fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto,MediaType.userImage);
                    var metadata = new Dictionary<string, string>
            {
                { "UserName", user.FullName ?? "User" }
            };

                    Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
                        post.userId,
                        NotificationType.postReacted,
                        post.id.ToString(),
                        image,
                        "en",
                        metadata
                    ));
                }
            }

            await _reactionReepository.SaveChangesAsync();
        }
        
        public async Task<bool> CheckReact(ToggleReactionDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            var existingReaction = await _reactionReepository.GetReaction(dto);

            return existingReaction != null;
        }
    }
}
