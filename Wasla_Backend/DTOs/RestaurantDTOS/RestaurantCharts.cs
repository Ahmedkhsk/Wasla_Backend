namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class RestaurantCharts
    {
        public int numberOfReservations { get; set; }
        public int numOfOrders { get; set; }
        public int numOfCompletedOrders { get; set; }
        public decimal totalAmount { get; set; }
        public List<CollectedPerYearDto> years { get; set; }
    }
}
