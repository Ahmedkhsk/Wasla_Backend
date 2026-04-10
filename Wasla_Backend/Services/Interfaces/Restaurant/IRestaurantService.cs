namespace Wasla_Backend.Services.Interfaces
{
    public interface IRestaurantService
    {
        public Task CompleteProfile(CompleteRegisterRestaurantDto dto);
    }
}
