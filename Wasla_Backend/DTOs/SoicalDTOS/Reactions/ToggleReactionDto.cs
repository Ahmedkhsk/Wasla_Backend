namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class ToggleReactionDto
    {
        public int targetId { get; set; }
        public ReactionTargetType targetType { get; set; } = ReactionTargetType.post;
        public ReactionType reactionType { get; set; }
        public string userId { get; set; }

    }
}
