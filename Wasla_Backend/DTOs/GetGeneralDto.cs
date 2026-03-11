namespace Wasla_Backend.DTOs
{
    public class GetGeneralDto<T> : PaginationParams
    {
        public string lan { get; set; } = "en";
        public T id { get; set; }
    }
}
