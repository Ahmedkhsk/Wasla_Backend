namespace Wasla_Backend.Services.Implementation.Files
{
    public class FileUrlBuilderService : IFileUrlBuilderService
    {
        private readonly FileStorageSettings _settings;

        public FileUrlBuilderService(IOptions<FileStorageSettings> options)
        {
            _settings = options.Value;
        }

        public string GetPath(MediaType type)
        {
            return type switch
            {
                MediaType.userImage => _settings.Paths.UserImages,
                MediaType.gymImage => _settings.Paths.GymImages,
                MediaType.doctorCV => _settings.Paths.DoctorCV,
                MediaType.bookingImage => _settings.Paths.BookingImages,
                MediaType.postFile => _settings.Paths.PostFiles,
                MediaType.qrCode => _settings.Paths.QrCodes,
                MediaType.MLModel => _settings.Paths.MLModels,
                MediaType.DriverFilePath => _settings.Paths.DriverFiles,
                MediaType.DriverCarImage => _settings.Paths.DriverCarImages,
                MediaType.chatFile => _settings.Paths.ChatFiles,
                MediaType.TechnicianDocument => _settings.Paths.TechnicianDocuments,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        public string? GetMediaUrl(string fileName, MediaType type)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            if (fileName.StartsWith("http"))
                return fileName;

            var path = GetPath(type);

            return $"{_settings.BaseUrl}/{path}/{fileName}";
        }
    }
}
