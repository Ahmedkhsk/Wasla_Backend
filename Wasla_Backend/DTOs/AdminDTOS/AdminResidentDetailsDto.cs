namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminResidentDetailsDto(Resident resident)
    {
        public string nationalId { get; set; } = resident.NationalId;
    }
}
