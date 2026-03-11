namespace Wasla_Backend.Mappings
{
    public class RideProfile:Profile
    {
        public RideProfile()
        {
            CreateMap<RequestRideDto, Ride>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ResidentId, opt => opt.MapFrom(src => src.PassengerId))
                ;
        }
    }
}
