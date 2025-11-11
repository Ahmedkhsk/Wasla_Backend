namespace Wasla_Backend.DTOs.ResidentDTOS
{
    public class ResidentCompleteRegisterDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string NationalId { get; set; }
        public IFormFile Image { get; set; }
        public string BirthDay { get; set; }


        public string Phone { get; set; }

    }
}
