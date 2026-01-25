namespace Wasla_Backend.DTOs.ChartDTOS
{
    public class CollectedPerYearDto
    {
        public int year { get; set; }
        public List<CollectedPerMonthDto> months { get; set; }
    }
}
