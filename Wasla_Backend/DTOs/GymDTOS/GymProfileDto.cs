namespace Wasla_Backend.DTOs.GymDTOS
{
    public class GymProfileDto
    {
        public string id { get; set; }
        public string businessName { get; set; }
        public string ownerName { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public List<string> phones { get; set; }
        public string profilePhoto { get; set; }
        public List<string> photos { get; set; }
        public int ReviewsCount { get; set; }
        public double rating { get; set; }

    }
}
