namespace Wasla_Backend.DTOs.GymDTOS
{
    public class ChartsResponse
    {
        public int numberOfBookings { get; set; }
        public int numberOfTrainees { get; set; }
        public decimal totalAmount { get; set; }
        public List<CollectedPerYearDto> years { get; set; }
    }
}
