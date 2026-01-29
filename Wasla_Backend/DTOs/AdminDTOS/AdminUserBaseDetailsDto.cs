namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class AdminUserBaseDetailsDto(ApplicationUser user)
    {
        public string phone { get; set; } = user.Phone;
        public string birthDay { get; set; } = user.BirthDay;
        public string profilePhoto { get; set; } = user.ProfilePhoto;
    }

}
