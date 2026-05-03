namespace Wasla_Backend.DTOs.DoctorDTO
{
    public class DoctorProfileResponse
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string specializationName { get; set; }
        public int experienceYears { get; set; }
        public string universityName { get; set; }
        public string hospitalname { get; set; }
        public int numberOfpatients { get; set; }
        public int BookingCount { get; set; }
        public int ReviewsCount { get; set; }
        public double graduationYear { get; set; }
        public string birthDay { get; set; }
        public string phone { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string cv { get; set; }
        public float rating { get; set; }
    }
}
