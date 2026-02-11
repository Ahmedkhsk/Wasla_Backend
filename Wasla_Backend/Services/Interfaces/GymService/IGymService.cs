namespace Wasla_Backend.Services.Interfaces.GymService
{
    public interface IGymService
    {
        public Task CompleteRegister(GymCompleteRegisterDto service);
        public Task<List<AllGymsDataDto>> AllGyms();
        public Task<GymProfileDto> GymProfile(string id);
    }
}
