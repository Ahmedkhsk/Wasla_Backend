namespace Wasla_Backend.DTOs.DriverDTOS
{
    public class DriverChartDto
    {
        public int numberOfRides { get; set; }
        public int numberOfDeliveredResident { get; set; }
        public double totalAmount { get; set; }
        public List<CollectedPerYearDto> years { get; set; }
    }

}
