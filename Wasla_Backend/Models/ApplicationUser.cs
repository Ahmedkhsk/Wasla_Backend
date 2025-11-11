namespace Wasla_Backend.Models
{
    public abstract class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; } 
        public string? ProfilePhoto { get; set; } 
        public string? Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public char Gender { get; set; }
        public string? BirthDay { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        public override string? UserName
        {
            get => Email;
            set { }
        }
        public override string? NormalizedUserName
        {
            get => Email?.ToUpper();
            set { }
        }

    }
}
