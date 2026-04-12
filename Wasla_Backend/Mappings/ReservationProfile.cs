namespace Wasla_Backend.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservations, GetReservationsToRestaurantResponse>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.user.FullName))
                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.user.Phone))
                .ForMember(dest => dest.profile, opt => opt.Ignore());
        }
    }
}
