namespace Wasla_Backend.DTOs
{
    public class SearchResponse
    {
        public int id { get; set; }
        public ServiceProviderType type { get; set; }
        public dynamic details { get; set; }
    }
}
