namespace Wasla_Backend.Models
{
    public class ContactUs
    {
        public string fullName { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string message { get; set; }
    }
}
