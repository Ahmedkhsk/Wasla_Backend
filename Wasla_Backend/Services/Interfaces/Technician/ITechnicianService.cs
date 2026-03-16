namespace Wasla_Backend.Services.Interfaces.technician
{
    public interface ITechnicianService
    {
        public Task CompleteRegisterAsync(TechnicianCompleteRegisterDto technicianCompleteRegisterDto);
        public Task<TechnicianProfileDto> GetProfileById(string id);
        public Task UpdateProfile(TechnicianUpdateProfileDto technicianUpdateProfileDto);
    }
}
