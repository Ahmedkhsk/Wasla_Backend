namespace Wasla_Backend.DTOs.PaginationDTOS
{
    public class GetGeneralWithPaginationDto<T> : PaginationParams
    {
        public T id { get; set; }
    }
}
