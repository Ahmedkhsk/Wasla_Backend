namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class UpdateQuantityDto : LanDto
    {
        public int cartItemId { get; set; }
        public string residentId { get; set; }
        public int quantity { get; set; }
    }
}
