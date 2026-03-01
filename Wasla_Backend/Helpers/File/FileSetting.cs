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

        public const string BaseUrl = "https://waslammka.runasp.net";

        public static string? GetMediaUrl(string fileName, MediaType type)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            string path = type switch
            {
                MediaType.userImage => ImagesPathUser,
                MediaType.gymImage => ImagesPathGym,
                MediaType.doctorCV => PathCVDoctor,
                MediaType.bookingImage => ImagesPathBooking,
                MediaType.postFile => FilesPosts,
                MediaType.qrCode => QrCodePath,
                MediaType.MLModel => MLModelsPath,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            return $"{BaseUrl}/{path.TrimStart('/')}/{fileName}";
        }
    }
}
