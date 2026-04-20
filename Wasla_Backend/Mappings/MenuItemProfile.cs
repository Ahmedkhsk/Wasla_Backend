namespace Wasla_Backend.Mappings
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, GetMenuItemDto>()
                .ForMember(dest => dest.nameValue, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.name.GetText(context.Items["lang"].ToString())
                ))
                .ForMember(dest => dest.categoryName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.category.name.GetText(context.Items["lang"].ToString())
                ));

            CreateMap<AddMenuItemDto, MenuItem>()
                .ForMember(dest => dest.imageUrl, opt => opt.Ignore());
            
            CreateMap<UpdateMenuItemDto, MenuItem>()
                .ForMember(dest => dest.imageUrl, opt => opt.Ignore());

        }
    }
}
