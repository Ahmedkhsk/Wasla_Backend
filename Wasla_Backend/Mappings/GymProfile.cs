using Wasla_Backend.Models.GymModel;

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
            
            CreateMap<AddPackageDto,Package>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.durationInMonths))
                .ForMember(dest => dest.Precentage, opt => opt.MapFrom(src => src.precentage));

            CreateMap<UpdatePackageDto, Package>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.Precentage, opt => opt.MapFrom(src => src.precentage));

            CreateMap<GymBookDto, GymBooking>()
                .ForMember(dest => dest.GymId, opt => opt.MapFrom(src => src.gymId))
                .ForMember(dest => dest.ResidentId, opt => opt.MapFrom(src => src.residentId))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.serviceId));

        }
    }
}
