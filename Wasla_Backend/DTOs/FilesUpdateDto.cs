namespace Wasla_Backend.DTOs
{
    public class FilesUpdateDto
    {
        public List<string>? existingFiles { get; set; }
        public List<IFormFile>? newFiles { get; set; }
    }
}
