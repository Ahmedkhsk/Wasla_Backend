namespace Wasla_Backend.Services.Implementation.Files
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileOperationService _fileOperationService;

        public FileService(IWebHostEnvironment env, IFileOperationService fileOperationService)
        {
            _env = env;
            _fileOperationService = fileOperationService;
        }

        private string GetPath(string folder)
            => Path.Combine(_env.WebRootPath, folder.TrimStart('/'));

        public async Task<string?> AddFileAsync(IFormFile? file, string folder)
        {
            if (file == null) return null;
            var path = GetPath(folder);
            Console.WriteLine(path);
            return await _fileOperationService.SaveFile(file, path);
        }

        public async Task<List<string>> AddFilesAsync(IEnumerable<IFormFile>? files, string folder)
        {
            if (files == null) return new List<string>();

            var path = GetPath(folder);

            var tasks = files.Select(file => _fileOperationService.SaveFile(file, path));

            return (await Task.WhenAll(tasks)).ToList();
        }

        public async Task<List<string>> ReplaceFilesAsync(
            IEnumerable<string>? oldFiles,
            IEnumerable<string>? existingFiles,
            IEnumerable<IFormFile>? newFiles,
            string folder)
        {
            var result = oldFiles?.ToList() ?? new List<string>();
            var filesToDelete = GetDeletedItems(oldFiles, existingFiles);
            DeleteFiles(filesToDelete, folder);
            result.RemoveAll(f => filesToDelete.Contains(f));
            if (newFiles != null)
                result.AddRange(await AddFilesAsync(newFiles, folder));
            return result;
        }

        public async Task<string?> ReplaceFileAsync(string? oldFile, IFormFile? newFile, string folder)
        {
            if (newFile == null)
                return oldFile;
            DeleteFile(oldFile, folder);
            return await AddFileAsync(newFile, folder);
        }

        public List<TItem> GetDeletedItems<TItem>(IEnumerable<TItem>? oldItems, IEnumerable<TItem>? newItems)
        {
            if (oldItems == null) return new();
            if (newItems == null) return oldItems.ToList();

            return oldItems.Except(newItems).ToList();
        }

        public List<string>? ExtractFileNames(IEnumerable<string>? urls)
        {
            return urls?.Select(u => Path.GetFileName(u)).ToList();
        }

        public void DeleteFile(string? file, string folder)
        {
            if (file == null) return;
            var path = GetPath(folder);
            _fileOperationService.DeleteFile(file, path);
        }

        public void DeleteFiles(IEnumerable<string>? files, string folder)
        {
            if (files == null) return;

            var path = GetPath(folder);

            foreach (var file in files)
                _fileOperationService.DeleteFile(file, path);
        }
    }
}