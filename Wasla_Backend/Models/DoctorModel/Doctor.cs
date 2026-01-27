namespace Wasla_Backend.Models
{
    public class Doctor : ServiceProvider
    {
        public int ExperienceYears { get; set; }
        public string? UniversityName { get; set; }
        public double GraduationYear { get; set; }
        public string? hospitalname { get; set; }
        public int numberOfpatients { get; set; }
        public ICollection<Service> services { get; set; }
        public DoctorSpecialization? Specialization { get; set; }

        [ForeignKey("Specialization")]
        public int? SpecializationId { get; set; }
    }
}
