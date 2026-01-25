namespace Wasla_Backend.DTOs.AdminDTOS
{
    public class ContactUsDto
    {
        public string fullName { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string message { get; set; }

    }
}
