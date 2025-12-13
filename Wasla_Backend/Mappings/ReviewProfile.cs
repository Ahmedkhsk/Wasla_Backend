namespace Wasla_Backend.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {

            CreateMap<AddReviewDto, Reviews>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.content))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.rating))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.userId))
                .ForMember(dest => dest.ServiceProviderId, opt => opt.MapFrom(src => src.serviceProviderId));


        }
    }
}
