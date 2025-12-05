namespace Wasla_Backend.DTOs.ResidentDTOS
{
    public class ResidentChartDto
    {
        public int numOfBookings { get; set; }
        public double totalAmount { get; set; }
        public List<ResidentYearDto> years { get; set; } = new();
    }
}
