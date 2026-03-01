namespace Wasla_Backend.DTOs.SoicalDTOS
{
    public class PostByUserIdResponse
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string profilePhoto { get; set; }
        public PagedResult<PostRespnse> posts { get; set; }
    }
}
