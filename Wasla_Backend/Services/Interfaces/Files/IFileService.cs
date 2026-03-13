namespace Wasla_Backend.Services.Interfaces.Files
{
    public interface IFileService
    {
        Task<string?> AddFileAsync(IFormFile? file, string folder);

        Task<List<string>> AddFilesAsync(IEnumerable<IFormFile>? files, string folder);

        public Task<List<string>> ReplaceFilesAsync(IEnumerable<string>? oldFiles, IEnumerable<string>? existingFiles, IEnumerable<IFormFile>? newFiles, string folder);
        Task<string?> ReplaceFileAsync(string? oldFile, IFormFile? newFile, string folder);
        public List<string>? ExtractFileNames(IEnumerable<string>? urls);
        void DeleteFiles(IEnumerable<string>? files, string folder);
        void DeleteFile(string? file, string folder);
    }
}
