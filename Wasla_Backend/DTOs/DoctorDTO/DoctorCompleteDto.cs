namespace Wasla_Backend.DTOs.DoctorDTO
{
    public class DoctorCompleteDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
        public int SpecializationId { get; set; }
        public int ExperienceYears { get; set; }
        public string UniversityName { get; set; }
        public double GraduationYear { get; set; }
        public string BirthDay { get; set; }
        public string Phone { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Description { get; set; }

        public IFormFile Image { get; set; }
        public IFormFile CV { get; set; }

    }
}
