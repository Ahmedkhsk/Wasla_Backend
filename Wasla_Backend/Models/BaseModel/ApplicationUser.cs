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
        public bool IsVerified { get; set; }=false;
        public bool IsCompleteRegistration { get; set; }=false;
        public int CountViolations { get; set; } = 0;
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime? lastSeen { get; set; }
        public bool isOnline { get; set; }
        public string? bio { get; set; } = "Hey there! I'm using Wasla.";
        public bool IsDeleted { get; set; } = false;
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
