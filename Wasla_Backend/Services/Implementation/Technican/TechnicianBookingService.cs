namespace Wasla_Backend.Services.Implementation.technican
{
    public class TechnicianBookingService : ITechnicianBookingService
    {
        private readonly ITechnicianBookingRepository _technicianBookingRepository;
        public TechnicianBookingService(ITechnicianBookingRepository technicianBookingRepository)
        {
            _technicianBookingRepository = technicianBookingRepository;
        }
    }
}
