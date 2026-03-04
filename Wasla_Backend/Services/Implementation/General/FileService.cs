namespace Wasla_Backend.Services.Implementation.General
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        private string GetPath(string folder)
            => Path.Combine(_env.WebRootPath, folder.TrimStart('/'));

        public async Task<string?> AddFileAsync(IFormFile? file, string folder)
        {
            if (file == null) return null;

            var path = GetPath(folder);
            Console.WriteLine(path);
            return await FileOperation.SaveFile(file, path);
        }

        public async Task<List<string>> AddFilesAsync(IEnumerable<IFormFile>? files, string folder)
        {
            var result = new List<string>();

            if (files == null) return result;

            var path = GetPath(folder);

            foreach (var file in files)
            {
                var saved = await FileOperation.SaveFile(file, path);
                result.Add(saved);
            }

            return result;
        }

        public async Task<List<string>?> ReplaceFilesAsync(IEnumerable<string>? oldFiles,IEnumerable<IFormFile>? newFiles,string folder,ReplaceFileMode mode)
        {
            var path = GetPath(folder);

            if (newFiles == null)
            {
                if (mode == ReplaceFileMode.ModelNotNullable)
                    return oldFiles?.ToList();

                if (mode == ReplaceFileMode.ModelNullable)
                {
                    DeleteFiles(oldFiles, folder);
                    return null;
                }
            }

            DeleteFiles(oldFiles, folder);

            return await AddFilesAsync(newFiles, folder);
        }
        public async Task<string?> ReplaceFileAsync(string? oldFile, IFormFile? newFile, string folder, ReplaceFileMode mode)
        {
            var path = GetPath(folder);

            if (newFile == null)
            {
                if (mode == ReplaceFileMode.ModelNotNullable)
                    return oldFile;

                if (mode == ReplaceFileMode.ModelNullable)
                {
                    DeleteFile(oldFile, folder);
                    return null;
                }
            }

            DeleteFile(oldFile, folder);

            return await AddFileAsync(newFile, folder);
        }

        public void DeleteFile(string? file, string folder)
        {
            if (file == null) return;

            var path = GetPath(folder);

            FileOperation.DeleteFile(file, path);
        }

        public void DeleteFiles(IEnumerable<string>? files, string folder)
        {
            if (files == null) return;

            var path = GetPath(folder);

            foreach (var file in files)
            {
                FileOperation.DeleteFile(file, path);
            }
        }
    }
}
