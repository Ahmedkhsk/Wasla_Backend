namespace Wasla_Backend.Services.Interfaces
{
    public interface IResidentService
    {
        public Task  CompleteResidentRegister(ResidentCompleteRegisterDto model);
    }
}
