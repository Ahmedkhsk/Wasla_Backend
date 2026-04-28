namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class ReportResponse
    {
        public int id { get; set; }
        public string reason { get; set; }
        public string userReportId { get; set; }
        public string? userReportProfile { get; set; }
        public string userNameReport { get; set; }
        public DateTime createdAt { get; set; }
    }
}
