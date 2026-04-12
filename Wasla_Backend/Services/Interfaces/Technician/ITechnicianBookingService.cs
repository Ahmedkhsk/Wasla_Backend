namespace Wasla_Backend.Services.Interfaces.technician
{
    public interface ITechnicianBookingService
    {
        public Task<int> RequestBooking(TechnicianBookingRequestDto request);
        public Task<BookingDetailsForTechnicianDto> GetBookingDetailsForTechnician(int bookingId);
        public Task AcceptBooking(int bookingId);
        public Task RejectBooking(int bookingId);
        public Task<List<BookingDetailsForTechnicianDto>> technicianBookingOfTechnician(string technicianId);
        public Task<List<TechnicianBookingOfResident>> technicianBookingOfResidents(string residentId);
        public Task<List<TechnicianBookingOfResident>> GetByResidentIdAndSpecialization(string residentId, TechnicianSpecialty specialization);

        public Task CancelBooking(int bookingId,bool IsResident);
    }
}
