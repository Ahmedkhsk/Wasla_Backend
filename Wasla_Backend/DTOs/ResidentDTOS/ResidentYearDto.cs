namespace Wasla_Backend.DTOs.ResidentDTOS
{
    public class ResidentYearDto
    {
        public int year { get; set; }
        public List<ResidentMonthDto> months { get; set; } = new();
    }
}