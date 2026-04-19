namespace Wasla_Backend.Mappings
{
    public class SocialProfile : Profile
    {
        public SocialProfile()
        {
            CreateMap<AddPostDto, Post>();

            CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.files, opt => opt.Ignore());
        }
    }
}
