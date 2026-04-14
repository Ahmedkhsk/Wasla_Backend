namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class UpdatePostDto
    {
        public int id { get; set; }
        public string? content { get; set; }
        public FilesUpdateDto files { get; set; }
    }
}
