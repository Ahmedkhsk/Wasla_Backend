namespace Wasla_Backend.Services.Interfaces.Files
{
    public interface IFileUrlBuilderService
    {
        string? GetMediaUrl(string fileName, MediaType type);
        string GetPath(MediaType type);
    }
}
