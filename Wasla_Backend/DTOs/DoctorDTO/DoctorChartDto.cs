
namespace Wasla_Backend.DTOs.DoctorDTO
{
    public class DoctorChartDto
    {
        public int numOfPatients { get; set; }
        public int numOfBookings { get; set; }
        public int numOfCompletedBookings { get; set; }
        public decimal totalAmount { get; set; }
        public List<CollectedPerYearDto> years { get; set; }

    }
}
