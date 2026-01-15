namespace Wasla_Backend.Helpers.File
{
    public static class FileOperation
    {
        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".pdf"
        };

        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "application/pdf"
        };

        private const long MaxFileSize = 5 * 1024 * 1024;

        public static async Task<string> SaveFile(IFormFile file, string filePath)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("FileIsRequired");

            if (file.Length > MaxFileSize)
                throw new BadRequestException("FileSizeExceeded");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new BadRequestException("InvalidFileType");

            if (!AllowedContentTypes.Contains(file.ContentType))
                throw new BadRequestException("InvalidFileContentType");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(filePath, fileName);

            using var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string filePath)
        {
            var fileUrl = Path.Combine(filePath, fileName);

            if (System.IO.File.Exists(fileUrl))
                System.IO.File.Delete(fileUrl);
        }
    }
}
