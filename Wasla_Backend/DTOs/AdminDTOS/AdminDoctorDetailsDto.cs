namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminDoctorDetailsDto(Doctor doctor)
    {
        public int experienceYears { get; set; } = doctor.ExperienceYears;
        public string universityName { get; set; } = doctor.UniversityName;
        public double graduationYear { get; set; } = doctor.GraduationYear;
        public string hospitalName { get; set; } = doctor.hospitalname;
        public string description { get; set; } = doctor.Description;
        public string CV { get; set; } = doctor.CV;
    }

}
