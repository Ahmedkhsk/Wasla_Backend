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

            CreateMap<UpdateRestaurantDto, Restaurant>()
            .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ownerName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phoneNumber))
            .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
            .ForMember(dest => dest.gallery, opt => opt.Ignore());

            CreateMap<Restaurant, GetAllRestaurantsResponse>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.BusinessName))
                .ForMember(dest => dest.ownerName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.profile, opt => opt.Ignore())
                .ForMember(dest => dest.gallery, opt => opt.Ignore());

            CreateMap<Restaurant, GetRestaurantResponse>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.BusinessName))
                .ForMember(dest => dest.ownerName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.phoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.restaurantCategoryId, opt => opt.MapFrom(src => src.restaurantCategoryId))
                .ForMember(dest => dest.restaurantCategoryName,
                opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items.TryGetValue("lang", out var value)
                        ? value?.ToString() ?? "en"
                        : "en";

                    return src.restaurantCategory?.name?.GetText(lang);
                }))                
                .ForMember(dest => dest.profile, opt => opt.Ignore())
                .ForMember(dest => dest.gallery, opt => opt.Ignore());
        }
    }
}
