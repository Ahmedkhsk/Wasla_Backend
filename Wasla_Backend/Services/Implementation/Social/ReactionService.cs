namespace Wasla_Backend.Services.Implementation
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionReepository;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IUserRepository _userRepository;

        public ReactionService(IReactionRepository reactionReepository,DateTimeHelper dateTimeHelper,IUserRepository userRepository) 
        {
            _reactionReepository = reactionReepository;
            _dateTimeHelper = dateTimeHelper;
            _userRepository = userRepository;
        }

        public async Task ToggleReaction(ToggleReactionDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var existingReaction = await _reactionReepository.GetReaction(dto.targetId, dto.type, dto.userId);
            
            if (existingReaction != null)
                 _reactionReepository.Delete(existingReaction);
            else
            {
                var newReaction = new Reaction
                {
                    targetId = dto.targetId,
                    targetType = dto.type,
                    userId = dto.userId,
                    createdAt = _dateTimeHelper.Now
                };

                await _reactionReepository.AddAsync(newReaction);
            }

            await _reactionReepository.SaveChangesAsync();
        }
        
        public async Task<bool> CheckReact(ToggleReactionDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            var existingReaction = await _reactionReepository.GetReaction(dto.targetId, dto.type, dto.userId);

            return existingReaction != null;
        }
    }
}
