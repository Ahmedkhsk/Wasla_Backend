namespace Wasla_Backend.Mappings
{
    public class ResidentProfile : Profile
    {
        public ResidentProfile()
        {
            CreateMap<ResidentCompleteRegisterDto, Resident>();
            CreateMap<EditProfileDto, Resident>()
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.fullname))
              .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
              .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.latitude))
              .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.longitude));

            CreateMap<Resident, ResponseProfileDto>()
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProfilePhoto))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

        }
    }
}
