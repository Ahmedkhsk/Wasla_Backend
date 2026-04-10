namespace Wasla_Backend.Mappings
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<CompleteRegisterRestaurantDto, Restaurant>()
            .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ownerName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phoneNumber))
            .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
            .ForMember(dest => dest.gallery, opt => opt.Ignore());

        }
    }
}
