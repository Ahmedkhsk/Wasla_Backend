using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Wasla_Backend.Data
{
    public class Context : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ResidentIdentity>residentIdentities { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<ServiceDate> ServiceDate { get; set; }
        public DbSet<ServiceDay> ServiceDay { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doctor>().ToTable("Doctor");
            builder.Entity<Driver>().ToTable("Driver");
            builder.Entity<Gym>().ToTable("Gym");
            builder.Entity<Resident>().ToTable("Resident");
            builder.Entity<Restaurant>().ToTable("Restaurant");

            builder.Owned<MultilingualText>();

            foreach (var fk in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                fk.DeleteBehavior = DeleteBehavior.NoAction;

            builder.Entity<Service>(entity =>
            {
                entity.OwnsOne(s => s.description, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("description_English");
                    sa.Property(p => p.Arabic).HasColumnName("description_Arabic");
                });

                entity.OwnsOne(s => s.serviceName, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("serviceName_English");
                    sa.Property(p => p.Arabic).HasColumnName("serviceName_Arabic");
                });
            });
        }
    }
}

