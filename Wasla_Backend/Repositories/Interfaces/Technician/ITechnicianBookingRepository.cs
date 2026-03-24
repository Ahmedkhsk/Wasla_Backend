namespace Wasla_Backend.Repositories.Interfaces.technician
{
    public interface ITechnicianBookingRepository : IGenericRepository<TechnicianBooking>
    {
        public Task<BookingDetailsForTechnicianDto> DetailsForTechnician(int bookingId);
        public Task<bool> IsExist(int bookingId);
        public Task<int> ChangeBookingStatus(int bookingId, TechnicianBookingStatus status);
        public Task<List<TechnicianBookingOfResident>> technicianBookingOfResidents(string residentId);
        public Task<List<BookingDetailsForTechnicianDto>> technicianBookingOfTechnician(string technicianId);
        public Task<List<TechnicianBookingOfResident>> GetByResidentIdAndSpecialization(string residentId, TechnicianSpecialty specialization);

    }
}
