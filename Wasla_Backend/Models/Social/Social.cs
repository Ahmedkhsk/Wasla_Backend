namespace Wasla_Backend.Models.Social
{
    public abstract class Social
    {
        public int id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }

        public bool isDeleted { get; set; } = false;
        public bool isHidden { get; set; } = false;
        public string? content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
