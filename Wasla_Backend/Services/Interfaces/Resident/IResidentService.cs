namespace Wasla_Backend.Services.Interfaces
{
    public interface IResidentService
    {
        public Task  CompleteResidentRegister(ResidentCompleteRegisterDto model);
        public Task EditProfile(EditProfileDto editProfileDto);

        public Task<ResponseProfileDto> GetProfile(string userId);
        Task<ResidentChartDto> GetResidentChartAsync(string residentId);
        public Task UploadIdentity(string NationalId, string gmail);

    }
}
