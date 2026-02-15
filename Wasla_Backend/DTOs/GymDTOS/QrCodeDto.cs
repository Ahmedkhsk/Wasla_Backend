namespace Wasla_Backend.DTOs.GymDTOS
{
    public class QrCodeDto
    {
        public string residentPhoto { get; set; }
        public string residentName { get; set; }
        public string gymName { get; set; }
        public string serviceName { get; set; }
        public DateTime bookingTime { get; set; }
        public DateTime expiryDate { get; set; }
    }
}
