namespace Wasla_Backend.DTOs.PaginationDTOS
{
    public class PaginationParams : LanDto
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? search { get; set; }
        public int? filterId { get; set; }
    }
}
