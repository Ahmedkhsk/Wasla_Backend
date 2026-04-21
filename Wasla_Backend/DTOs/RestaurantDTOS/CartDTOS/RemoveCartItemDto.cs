namespace Wasla_Backend.DTOs.RestaurantDTOS
{ 
    public class RemoveCartItemDto : LanDto
    {
        public int cartItemId { get; set; }
        public string residentId { get; set; }
    }
}
