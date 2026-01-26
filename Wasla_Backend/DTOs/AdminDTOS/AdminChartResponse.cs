namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminChartResponse
    {
        public int completedBookingsCount { get; set; }
        public int canceledBookingsCount { get; set; }
        public List<CollectedPerYearDto> years { get; set; } = new List<CollectedPerYearDto>();
    }
}
