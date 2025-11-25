namespace Wasla_Backend.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile() 
        {
            CreateMap<DoctorCompleteDto, Doctor>();

            CreateMap<Doctor, DoctorProfileResponse>()
            .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.ProfilePhoto))
            .ForMember(dest => dest.latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.birthDay, opt => opt.MapFrom(src => src.BirthDay))
            .ForMember(dest => dest.cv, opt => opt.MapFrom(src => src.CV))
            .ForMember(dest => dest.universityName, opt => opt.MapFrom(src => src.UniversityName)) 
            .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.experienceYears, opt => opt.MapFrom(src => src.ExperienceYears))
            .ForMember(dest => dest.graduationYear, opt => opt.MapFrom(src => src.GraduationYear))
            .ForMember(dest => dest.specializationName,
                       opt => opt.MapFrom((src, dest, destMember, context) =>
                       src.Specialization.Specialization.GetText(context.Items["lan"].ToString())));

            CreateMap<Doctor, AllDoctorDataDto>()
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.ProfilePhoto))
                .ForMember(d => d.CVUrl, o => o.MapFrom(s => s.CV));
            


        }
    }
}
