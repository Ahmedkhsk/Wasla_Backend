namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IFileService
    {
        Task<string?> AddFileAsync(IFormFile? file, string folder);

        Task<List<string>> AddFilesAsync(IEnumerable<IFormFile>? files, string folder);

        Task<List<string>?> ReplaceFilesAsync(IEnumerable<string>? oldFiles,IEnumerable<IFormFile>? newFiles,string folder,ReplaceFileMode mode);
        Task<string?> ReplaceFileAsync(string? oldFile, IFormFile? newFile, string folder, ReplaceFileMode mode);
        
        void DeleteFiles(IEnumerable<string>? files, string folder);
        void DeleteFile(string? file, string folder);
    }
}
