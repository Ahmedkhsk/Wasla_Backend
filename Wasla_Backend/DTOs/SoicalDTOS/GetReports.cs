namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class GetReports
    {
        public int targetId { get; set; }
        public ReactionTargetType targetType { get; set; }
        public string? content { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string? image { get; set; }
        public List<string>? images { get; set; }
        public int countReports { get; set; }
        public List<ReportResponse> reports { get; set; }
    }
}
