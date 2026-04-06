namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class TechnicianChartDto
    {
        public int CompletedBookings { get; set; }
        public int NumberOfResidents { get; set; }
        public double totalAmount { get; set; }
        public List<CollectedPerYearDto> years { get; set; }
    }
}
