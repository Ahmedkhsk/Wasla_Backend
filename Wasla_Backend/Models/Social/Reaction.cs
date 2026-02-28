namespace Wasla_Backend.Models.Social
{
    public class Reaction
    {
        public int id { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }

        public int targetId { get; set; }

        public ReactionTargetType targetType { get; set; }

        public DateTime createdAt { get; set; }
    }
}
