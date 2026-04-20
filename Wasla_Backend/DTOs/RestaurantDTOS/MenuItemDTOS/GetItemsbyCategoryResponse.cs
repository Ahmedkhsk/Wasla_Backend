namespace Wasla_Backend.DTOs.RestaurantDTOS
{
    public class GetItemsbyCategoryResponse
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<ItemResponse> items { get; set; }
    }
}
