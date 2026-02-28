namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class ToggleReactionDto
    {
        public int targetId { get; set; }
        public ReactionTargetType type { get; set; }
        public string userId { get; set; }

    }
}
