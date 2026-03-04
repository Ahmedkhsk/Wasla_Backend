namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class GetPostsByUsingReactionTypeDto
    {
        public string userId { get; set; }
        public ReactionType reactionType { get; set; }
        public string lan { get; set; } = "en";
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
