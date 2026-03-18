namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class AddReportDto : LanDto
    {
        public string userId { get; set; }
        public string reason { get; set; } = null!;
        public int targetId { get; set; }
        public ReactionTargetType targetType { get; set; }
    }
}
