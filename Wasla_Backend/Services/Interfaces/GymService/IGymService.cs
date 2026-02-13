namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IGymService
    {
        public Task CompleteRegister(GymCompleteRegisterDto service);
        public Task<List<AllGymsDataDto>> AllGyms(int pageNumber, int pageSize);
        public Task<GymProfileDto> GymProfile(string id);
    }
}
