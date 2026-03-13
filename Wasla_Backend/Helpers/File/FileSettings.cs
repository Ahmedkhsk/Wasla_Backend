namespace Wasla_Backend.Helpers.File
{
    public class FileSettings
    {
        public long MaxFileSize { get; set; }
        public string[] AllowedExtensions { get; set; }
        public string[] AllowedContentTypes { get; set; }
    }
}
