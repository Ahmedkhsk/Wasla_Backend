namespace Wasla_Backend.Services.Interfaces.technician
{
    public interface ITechnicianService
    {
        public Task CompleteRegisterAsync(TechnicianCompleteRegisterDto technicianCompleteRegisterDto);
        public Task<TechnicianProfileDto> GetProfileById(string id);
        public Task UpdateProfile(TechnicianUpdateProfileDto technicianUpdateProfileDto);
        public List<TechnicianSpecializationDto> GetSpecializations(string lan);
        public Task<PagedResult<TechnicianListDto>> GetTechniciansBySpecialty(TechnicianSpecialty? specialty,int pageNumber,int pageSize, string lan);
    }
}
