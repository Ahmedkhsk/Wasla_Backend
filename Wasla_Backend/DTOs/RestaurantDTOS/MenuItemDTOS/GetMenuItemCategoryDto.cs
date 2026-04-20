namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetMenuItemCategoryDto
    {
        public int id { get; set; }
        public MultilingualText name { get; set; }
        public string nameValue { get; set; }
        public string restaurantId { get; set; }
    }
}
