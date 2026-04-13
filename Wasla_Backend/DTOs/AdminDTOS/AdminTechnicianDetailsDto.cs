namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminTechnicianDetailsDto (Technician technician)
    {
        public string description { get; set; } = technician.Description;
        public int? experienceYears { get; set; } = technician.ExperienceYears;
        public TechnicianSpecialty? specialization { get; set; } = technician.Specialty;
        public double rate { get; set; } = technician.Rating;
        public bool isAvailable { get; set; } = technician.IsAvailable;
        public List<string> documents { get; set; } = technician.Documents;

    }
}
