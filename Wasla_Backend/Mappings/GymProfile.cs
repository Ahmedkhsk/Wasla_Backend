namespace Wasla_Backend.Mappings
{
    public class GymProfile : Profile
    {
        public GymProfile()
        {
            CreateMap<GymCompleteRegisterDto, Gym>()
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.businessName))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.ownerName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.longitude));
        }
    }
}
