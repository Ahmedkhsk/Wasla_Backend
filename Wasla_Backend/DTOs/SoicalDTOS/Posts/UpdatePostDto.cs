namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class UpdatePostDto
    {
        public int id { get; set; }
        public string? content { get; set; }

        public List<string>? existingFiles { get; set; }

        public List<IFormFile>? newFiles { get; set; }
    }
}
