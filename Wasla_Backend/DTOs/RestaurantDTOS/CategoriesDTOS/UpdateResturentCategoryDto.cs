namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class UpdateResturentCategoryDto : LanDto
    {
        public int id { get; set; }
        public MultilingualText name { get; set; }
    }
}
