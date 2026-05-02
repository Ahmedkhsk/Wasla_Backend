namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class ToggleHideDto : GetGeneralDto<int>
    {
        public string? reason { get; set; }
    }
}
