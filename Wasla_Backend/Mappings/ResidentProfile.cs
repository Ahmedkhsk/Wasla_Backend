namespace Wasla_Backend.Mappings
{
    public class ResidentProfile : Profile
    {
        public ResidentProfile()
        {
            CreateMap<ResidentCompleteRegisterDto, Resident>();
        }
    }
}
