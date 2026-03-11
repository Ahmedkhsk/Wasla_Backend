namespace Wasla_Backend.Helpers.File
{
   public static class FileSetting
    {
        public const string ImagesPathUser = "/assets/images/user";
        public const string ImagesPathGym = "/assets/images/gym";
        public const string PathCVDoctor = "/assets/cv/doctor";
        public const string ImagesPathBooking = "/assets/images/booking";
        public const string MLModelsPath = "/assets/ai/models";
        public const string QrCodePath = "/assets/qrcodes";
        public const string FilesPosts = "/assets/files/social";
        public const string DriverFilePath = "/assets/files/driver";
        public const string DriverCarImagesPath = "/assets/images/driver";
        public const string FilesChat = "/assets/files/chat";

        public const string BaseUrl = "https://waslammka.runasp.net";

        public static string? GetMediaUrl(string fileName, MediaType type)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            if (fileName.StartsWith("http://") || fileName.StartsWith("https://"))
                return fileName;

            string path = type switch
            {
                MediaType.userImage => ImagesPathUser,
                MediaType.gymImage => ImagesPathGym,
                MediaType.doctorCV => PathCVDoctor,
                MediaType.bookingImage => ImagesPathBooking,
                MediaType.postFile => FilesPosts,
                MediaType.qrCode => QrCodePath,
                MediaType.MLModel => MLModelsPath,
                MediaType.DriverFilePath => DriverFilePath,
                MediaType.DriverCarImage => DriverCarImagesPath,
                MediaType.chatFile => FilesChat,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            return $"{BaseUrl}/{path.TrimStart('/')}/{fileName}";
        }
    }
}
