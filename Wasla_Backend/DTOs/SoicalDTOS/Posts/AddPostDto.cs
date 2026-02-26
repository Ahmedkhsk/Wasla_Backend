namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class AddPostDto
    {
        public string userId { get; set; }
        public string content { get; set; }
        public List<FormFile>? files { get; set; }
    }
}
