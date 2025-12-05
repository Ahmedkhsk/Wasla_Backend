namespace Wasla_Backend.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile() 
        {
            CreateMap<UpdateBookingDto, Booking>();
        }    
    }
}
