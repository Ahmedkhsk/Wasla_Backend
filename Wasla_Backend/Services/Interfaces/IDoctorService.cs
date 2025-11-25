
namespace Wasla_Backend.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task CompleteData(DoctorCompleteDto doctorCompleteDto);
        public Task<IEnumerable<DoctorSpecializationResponse>> DoctorSpecializations(string lan);
        public Task<DoctorProfileResponse> GetDoctorProfile(string id, string lan);
        public Task<IEnumerable<AllDoctorDataDto>> GetAllDoctors(string lan);
        public Task<IEnumerable<AllDoctorDataDto>>GetDoctorBySpecialist(int specialistId,string lan);

    }
}
