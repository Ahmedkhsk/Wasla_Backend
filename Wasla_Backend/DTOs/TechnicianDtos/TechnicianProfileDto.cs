namespace Wasla_Backend.DTOs.TechnicianDtos
{
    public class TechnicianProfileDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string BirthDay { get; set; }
        public int? ExperienceYears { get; set; }
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public TechnicianSpecialty? Specialty { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public List<string> DocumentsUrls { get; set; }
        public double Rate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
