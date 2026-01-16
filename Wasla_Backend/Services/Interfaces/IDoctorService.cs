namespace Wasla_Backend.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task CompleteData(DoctorCompleteDto doctorCompleteDto);
        public Task UpdateDoctorProfile(UpdateDoctorDto updateDoctorDto);
        public Task<IEnumerable<DoctorSpecializationResponse>> DoctorSpecializations(string lan);
        public Task<DoctorProfileResponse> GetDoctorProfile(string id, string lan);
        public Task<IEnumerable<AllDoctorDataDto>> GetAllDoctors(string lan);
        public Task<DoctorChartDto> GetDoctorChart(string doctorId);
        public Task<List<GetAllBookingResponse>> GetAllBookingOfDoctors(string docId, BookingStatus status, string lan);
        public Task<IEnumerable<AllDoctorDataDto>>GetDoctorBySpecialist(int specialistId,string lan);
        public Task<AllDoctorDataDto> GetDoctorData(String doctorId,string lan);

    }
}
