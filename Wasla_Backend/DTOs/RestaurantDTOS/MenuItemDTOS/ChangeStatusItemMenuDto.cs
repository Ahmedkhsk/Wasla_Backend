namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class ChangeStatusItemMenuDto : LanDto
    {
        public string restaurantId { get; set; }
        public int menuItemId { get; set; }
    }
}
