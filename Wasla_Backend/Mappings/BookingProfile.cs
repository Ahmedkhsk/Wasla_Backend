namespace Wasla_Backend.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile() 
        {
            CreateMap<UpdateBookingDto, Booking>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.bookingDate));
        }    
    }
}
