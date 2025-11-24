namespace Wasla_Backend.Data
{
    public class Context : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ResidentIdentity> residentIdentities { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<ServiceDate> ServiceDate { get; set; }
        public DbSet<ServiceDay> ServiceDay { get; set; }
        public DbSet<Favorites> Favorite { get; set; }
        public DbSet<Reviews> Review { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doctor>().ToTable("Doctor");
            builder.Entity<Driver>().ToTable("Driver");
            builder.Entity<Gym>().ToTable("Gym");
            builder.Entity<Resident>().ToTable("Resident");
            builder.Entity<Restaurant>().ToTable("Restaurant");

            builder.Entity<DoctorSpecialization>(entity =>
            {
                entity.OwnsOne(d => d.Specialization, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Specialization_English");
                    sa.Property(p => p.Arabic).HasColumnName("Specialization_Arabic");
                    sa.WithOwner();
                });
            });


            builder.Entity<ApplicationRole>(entity =>
            {
                entity.OwnsOne(r => r.RoleName, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("RoleName_English");
                    sa.Property(p => p.Arabic).HasColumnName("RoleName_Arabic");
                    sa.WithOwner();
                });
            });

            builder.Entity<Service>(entity =>
            {
                entity.OwnsOne(s => s.description, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("description_English");
                    sa.Property(p => p.Arabic).HasColumnName("description_Arabic");
                    sa.WithOwner();
                });

                entity.OwnsOne(s => s.serviceName, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("serviceName_English");
                    sa.Property(p => p.Arabic).HasColumnName("serviceName_Arabic");
                    sa.WithOwner();
                });
            });
        }
    }
}
