namespace Wasla_Backend.Services.Implementation.Files
{
    public class FileOperationService : IFileOperationService
    {
        private readonly FileValidator _validator;

        public FileOperationService(FileValidator validator)
        {
            _validator = validator;
        }

        public async Task<string> SaveFile(IFormFile file, string filePath)
        {
            _validator.Validate(file);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(filePath, fileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public void DeleteFile(string fileName, string filePath)
        {
            var path = Path.Combine(filePath, fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
