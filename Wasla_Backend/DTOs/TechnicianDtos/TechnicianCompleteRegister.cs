namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class TechnicianCompleteRegisterDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string BirthDay { get; set; }
        public int ExperienceYears { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public TechnicianSpecialty Specialty { get; set; }
        public IFormFile Photo { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}
