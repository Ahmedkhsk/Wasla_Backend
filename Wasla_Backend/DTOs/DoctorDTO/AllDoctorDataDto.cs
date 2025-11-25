namespace Wasla_Backend.DTOs.DoctorDTO
{
    public class AllDoctorDataDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public float Rating { get; set; }
        public string UniversityName { get; set; }
        public double GraduationYear { get; set; }
        public string hospitalname { get; set; }
        public int NumberOfPatient { get; set; }
       public string BirthDay { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CVUrl { get; set; }
    }
}
