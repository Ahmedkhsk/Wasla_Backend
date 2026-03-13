namespace Wasla_Backend.Helpers.File
{
    public class FileValidator
    {
        private readonly FileSettings _settings;

        public FileValidator(IOptions<FileSettings> options)
        {
            _settings = options.Value;
        }

        public void Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(LocalizationKey.FileIsRequired);

            if (file.Length > _settings.MaxFileSize)
                throw new BadRequestException(LocalizationKey.FileSizeExceeded);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_settings.AllowedExtensions.Contains(extension))
                throw new BadRequestException(LocalizationKey.InvalidFileType);

            if (!_settings.AllowedContentTypes.Contains(file.ContentType))
                throw new BadRequestException(LocalizationKey.InvalidFileContentType);
        }
    }
}
