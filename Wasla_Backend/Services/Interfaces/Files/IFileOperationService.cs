namespace Wasla_Backend.Services.Interfaces.Files
{
    public interface IFileOperationService
    {
        public Task<string> SaveFile(IFormFile file, string filePath);
        public void DeleteFile(string fileName, string filePath);
    }
}
