namespace Wasla_Backend.DTOs
{
    public class GetGeneralDto<T> : PaginationParams
    {
        public T id { get; set; }
    }
}
