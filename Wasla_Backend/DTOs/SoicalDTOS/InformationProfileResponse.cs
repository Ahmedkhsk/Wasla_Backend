namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class InformationProfileResponse
    {
        public string userName { get; set; }
        public string profilePhoto { get; set; }
        public int postsCount { get; set; }
        public int reactionsCount { get; set; }
        public int savesCount { get; set; }
    }
}
