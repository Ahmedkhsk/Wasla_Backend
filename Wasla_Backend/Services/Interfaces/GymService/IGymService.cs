namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IGymService
    {
        public Task CompleteRegister(GymCompleteRegisterDto service);
        public Task<PagedResult<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize);
        public Task UpdateProfile(UpdateProfileGym dto);
        public Task<GymProfileDto> GymProfile(string id);
    }
}
