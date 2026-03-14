namespace Wasla_Backend.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<DriverCompleteRegisterDto, DriverModel>();
            CreateMap<UpdateDriverProfileDto, DriverModel>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.images, opt => opt.Ignore())
                .ForMember(dest => dest.DriverFiles, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
