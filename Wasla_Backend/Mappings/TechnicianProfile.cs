namespace Wasla_Backend.Mappings
{
    public class TechnicianProfile: Profile
    {
        public TechnicianProfile()
        {
            CreateMap<TechnicianCompleteRegisterDto, Technician>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore());
            CreateMap<TechnicianUpdateProfileDto, Technician>()
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore())
                .ForMember(dest => dest.Documents, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            ;
        }
    }
}
