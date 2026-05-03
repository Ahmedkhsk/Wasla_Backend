namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class ToggleHideDto : GetGeneralDto<int>
    {
        public string? reason { get; set; }
        public string adminId { get; set; }
    }
}
