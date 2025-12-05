namespace Wasla_Backend.DTOs.DoctorDTO
{
    public class UpdateDoctorDto
    {
        public string userId { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string birthDay { get; set; }
        public int experienceYears { get; set; }
        public string universityName { get; set; }
        public double graduationYear { get; set; }
        public string hospitalName { get; set; }
        public int specializationId { get; set; }
        public IFormFile? profilePhoto { get; set; }
        public IFormFile? cv { get; set; }
    }
}
