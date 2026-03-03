namespace Wasla_Backend.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<DriverCompleteRegisterDto, Driver>();
        }
    }
}
