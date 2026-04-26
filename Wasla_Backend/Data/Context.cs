using Notification = Wasla_Backend.Models.GeneralModel.Notification;

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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ResidentIdentity> residentIdentities { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceDay> ServiceDay { get; set; }
        public DbSet<Favourites> Favorite { get; set; }
        public DbSet<Reviews> Review { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<BaseBooking> BaseBookings { get; set; }    
        public DbSet<BaseService> BaseServices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<GymBooking> GymBooking { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<ride> rides { get; set; }
        public DbSet<RideDispatchJob> RideDispatchJobs { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TechnicianBooking> TechnicianBookings { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ServiceProvider> serviceProvider { get; set; }


        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doctor>().ToTable("Doctor");
            builder.Entity<Driver>().ToTable("Driver");
            builder.Entity<Gym>().ToTable("Gym");
            builder.Entity<Resident>().ToTable("Resident");
            builder.Entity<Restaurant>().ToTable("Restaurant");
            builder.Entity<BaseService>().ToTable("BaseServices");
            builder.Entity<Package>().ToTable("Packages");
            builder.Entity<GymBooking>().ToTable("GymBookings");
            builder.Entity<Payment>().ToTable("Payments");
            builder.Entity<ride>().ToTable("Rides");
            builder.Entity<Technician>().ToTable("Technicians");
            builder.Entity<TechnicianBooking>().ToTable("TechnicianBookings");
            builder.Entity<Service>().ToTable("DoctorServices");
            builder.Entity<Booking>().ToTable("DoctorBookings");
            builder.Entity<BaseBooking>().ToTable("BaseBookings");
            builder.Entity<Booking>().ToTable("DoctorBookings");


            builder.Entity<BaseBooking>()
                .HasOne(b => b.Resident)
                .WithMany()
                .HasForeignKey(b => b.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<GymBooking>()
                .HasOne(b => b.Gym)
                .WithMany() 
                .HasForeignKey(b => b.GymId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<GymBooking>()
                .HasOne(b => b.Service)
                .WithMany()
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GymBooking>()
                .HasOne(b => b.Resident)
                .WithMany()
                .HasForeignKey(b => b.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Package>(entity =>
            {
                entity.OwnsOne(p => p.Name, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Name_English");
                    sa.Property(p => p.Arabic).HasColumnName("Name_Arabic");
                    sa.WithOwner();
                });

                entity.OwnsOne(p => p.Description, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Description_English");
                    sa.Property(p => p.Arabic).HasColumnName("Description_Arabic");
                    sa.WithOwner();
                });
            });


            builder.Entity<DoctorSpecialization>(entity =>
            {
                entity.OwnsOne(d => d.Specialization, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Specialization_English");
                    sa.Property(p => p.Arabic).HasColumnName("Specialization_Arabic");
                    sa.WithOwner();
                });
            });

            builder.Entity<RestaurantCategory>(entity =>
            {
                entity.OwnsOne(d => d.name, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Name_English");
                    sa.Property(p => p.Arabic).HasColumnName("Name_Arabic");
                    sa.WithOwner();
                });
            });

            builder.Entity<MenuItem>(entity =>
            {
                entity.OwnsOne(d => d.name, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Name_English");
                    sa.Property(p => p.Arabic).HasColumnName("Name_Arabic");
                    sa.WithOwner();
                });
            });

            builder.Entity<MenuItemCategory>(entity =>
            {
                entity.OwnsOne(d => d.name, sa =>
                {
                    sa.Property(p => p.English).HasColumnName("Name_English");
                    sa.Property(p => p.Arabic).HasColumnName("Name_Arabic");
                    sa.WithOwner();
                });
            });

            builder.Entity<Booking>()
                .HasOne(b => b.serviceDay)
                .WithMany()
                .HasForeignKey(b => b.serviceDayId)
                .OnDelete(DeleteBehavior.Restrict);

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
            
            builder.Entity<BaseService>().HasQueryFilter(s=>!s.IsDeleted &&!s.IsHidden);
            
            builder.Entity<Payment>()
                 .HasOne(p => p.Resident)
                 .WithMany()
                 .HasForeignKey(p => p.ResidentId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.ServiceProvider)
                .WithMany()
                .HasForeignKey(p => p.ServiceProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Service)
                .WithMany()
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reviews>()
                .HasOne(r => r.ServiceProvider)
                .WithMany(sp => sp.Reviews)
                .HasForeignKey(r => r.ServiceProviderId)
                .OnDelete(DeleteBehavior.Restrict); 
            builder.Entity<Notification>().HasQueryFilter(n => !n.IsDeleted);
        
        }
    
    }
}
