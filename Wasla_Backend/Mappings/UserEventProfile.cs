namespace Wasla_Backend.Mappings
{
    public class UserEventProfile : Profile
    {
        public UserEventProfile()
        {
            CreateMap<UserEventDto, UserEvent>();
        }

    }
}
